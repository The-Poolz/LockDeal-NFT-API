using System.Reflection;
using Net.Urlify.Attributes;
using System.Reflection.Emit;
using MetaDataAPI.Providers.Image.Models;
using MetaDataAPI.Providers.PoolInformation;
using PropertyInfo = MetaDataAPI.Providers.Image.Models.PropertyInfo;

namespace MetaDataAPI.Providers.Image.Services;

public class DynamicTypeBuilder
{
    public BaseUrlifyModel CreateUrlifyModel(PoolInfo poolInfo)
    {
        var type = CreateDynamicType(poolInfo.UrlifyModelCreation);
        return (BaseUrlifyModel)Activator.CreateInstance(type, poolInfo)!;
    }

    public static Type CreateDynamicType(UrlifyModelCreation modelCreation)
    {
        var assemblyBuilder = CreateAssemblyBuilder();
        var moduleBuilder = CreateModuleBuilder(assemblyBuilder);

        var baseType = typeof(PoolInfo);
        var typeBuilder = DefineType(moduleBuilder, modelCreation.ClassName);

        AddConstructor(typeBuilder, baseType);
        AddProperties(typeBuilder, modelCreation.Properties);

        return typeBuilder.CreateType()!;
    }

    private static AssemblyBuilder CreateAssemblyBuilder()
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName();
        return AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
    }

    private static ModuleBuilder CreateModuleBuilder(AssemblyBuilder assemblyBuilder)
    {
        return assemblyBuilder.DefineDynamicModule("MainModule");
    }

    private static TypeBuilder DefineType(ModuleBuilder moduleBuilder, string className)
    {
        return moduleBuilder.DefineType(className, TypeAttributes.Public, typeof(BaseUrlifyModel));
    }

    private static void AddConstructor(TypeBuilder typeBuilder, Type baseType)
    {
        var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { baseType });
        var ctorIL = constructorBuilder.GetILGenerator();
        ctorIL.Emit(OpCodes.Ldarg_0);
        ctorIL.Emit(OpCodes.Ldarg_1);

        var ctorType = typeof(BaseUrlifyModel);
        var ctorParams = new[] { baseType };
        var ctor = ctorType.GetConstructor(ctorParams)
            ?? throw new InvalidOperationException(ConstructorNotFound(ctorType, ctorParams));

        ctorIL.Emit(OpCodes.Call, ctor);
        ctorIL.Emit(OpCodes.Ret);
    }

    private static void AddProperties(TypeBuilder typeBuilder, IEnumerable<PropertyInfo> properties)
    {
        properties.ToList().ForEach(prop => AddProperty(typeBuilder, prop));
    }

    private static void AddProperty(TypeBuilder typeBuilder, PropertyInfo prop)
    {
        var propertyBuilder = typeBuilder.DefineProperty(prop.Name, PropertyAttributes.HasDefault, typeof(string), null);
        var getPropMthdBldr = typeBuilder.DefineMethod(
            "get_" + prop.Name,
            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
            typeof(string),
            Type.EmptyTypes
        );

        var getIl = getPropMthdBldr.GetILGenerator();
        getIl.Emit(OpCodes.Ldstr, prop.Value);
        getIl.Emit(OpCodes.Ret);

        propertyBuilder.SetGetMethod(getPropMthdBldr);

        var ctorType = typeof(QueryStringPropertyAttribute);
        var ctorParams = new[] { typeof(string), typeof(bool), typeof(int) };
        var attributeCtor = ctorType.GetConstructor(ctorParams)
            ?? throw new InvalidOperationException(ConstructorNotFound(ctorType, ctorParams));

        var attributeBuilder = new CustomAttributeBuilder(attributeCtor, new object[] { prop.Name, false, prop.Order });
        propertyBuilder.SetCustomAttribute(attributeBuilder);
    }

    private static string ConstructorNotFound(Type typeOfCtor, IEnumerable<Type> ctorParams) =>
        $"Cannot find constructor for {typeOfCtor} with followed parameters: " +
        $"{Environment.NewLine}{string.Join(", ", ctorParams.Select(x => $"    {x.Name}"))}";
}