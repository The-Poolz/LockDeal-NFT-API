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
        var type = CreateDynamicType(
            poolInfo.UrlifyModelCreation.ClassName,
            poolInfo.UrlifyModelCreation.Properties);
        return (BaseUrlifyModel)Activator.CreateInstance(type, poolInfo)!;
    }

    public Type CreateDynamicType(string className, IEnumerable<PropertyInfo> properties)
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName();
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

        var baseType = Type.GetType(typeof(PoolInfo).FullName!);
        var typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public, typeof(BaseUrlifyModel));

        // Добавление конструктора
        var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { baseType });
        var ctorIL = constructorBuilder.GetILGenerator();
        ctorIL.Emit(OpCodes.Ldarg_0); // Загрузка this
        ctorIL.Emit(OpCodes.Ldarg_1); // Загрузка первого аргумента
        ctorIL.Emit(OpCodes.Call, typeof(BaseUrlifyModel).GetConstructor(new[] { baseType })); // Вызов базового конструктора
        ctorIL.Emit(OpCodes.Ret);

        foreach (var prop in properties)
        {
            // Добавление свойства
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(prop.Name, PropertyAttributes.HasDefault, prop.Type, null);
            MethodBuilder getPropMthdBldr = typeBuilder.DefineMethod("get_" + prop.Name,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                prop.Type, Type.EmptyTypes);

            ILGenerator getIl = getPropMthdBldr.GetILGenerator();
            getIl.Emit(OpCodes.Ldstr, prop.Value);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr = typeBuilder.DefineMethod("set_" + prop.Name,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null, new Type[] { prop.Type });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            setIl.Emit(OpCodes.Ret);  // Заглушка, здесь должен быть код для установки значения свойства

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);

            // Добавление атрибута к свойству
            ConstructorInfo attributeConstructor = typeof(QueryStringPropertyAttribute).GetConstructor(new Type[] { typeof(string), typeof(bool), typeof(int) });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(attributeConstructor, new object[] { prop.Name, false, prop.Order });
            propertyBuilder.SetCustomAttribute(attributeBuilder);
        }

        return typeBuilder.CreateTypeInfo();
    }
}