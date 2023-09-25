using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Models.Types;

namespace MetaDataAPI.Models.Response.Converters;

public class Erc721AttributeConverter : JsonConverter<Erc721Attribute>
{
    public override Erc721Attribute ReadJson(JsonReader reader, Type objectType, Erc721Attribute? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var traitType = jsonObject["trait_type"]!.Value<string>()!;
        var value = jsonObject["value"]!.ToObject<object>()!;
        var displayTypeString = jsonObject["display_type"]?.Value<string>();

        var displayType = DisplayType.String;
        if (displayTypeString != null)
        {
            Enum.TryParse(displayTypeString, true, out displayType);
        }

        return new Erc721Attribute(traitType, value, displayType);
    }

    public override void WriteJson(JsonWriter writer, Erc721Attribute? value, JsonSerializer serializer)
    {
        var obj = new JObject
        {
            ["trait_type"] = value!.TraitType,
            ["value"] = JToken.FromObject(value.Value)
        };

        if (value.DisplayType != DisplayType.String)
        {
            obj["display_type"] = value.DisplayType.ToString().ToLowerInvariant();
        }

        obj.WriteTo(writer);
    }
}
