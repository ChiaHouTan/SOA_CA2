using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
public class EnumStringConverter : StringEnumConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is Enum enumValue)
        {
            writer.WriteValue(enumValue.ToString("G"));
        }
        else
        {
            base.WriteJson(writer, value, serializer);
        }
    }
}
