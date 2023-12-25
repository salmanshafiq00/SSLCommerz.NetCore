using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class CustomDateTimeConverter : IsoDateTimeConverter
{
    public CustomDateTimeConverter()
    {
        DateTimeFormat = "yyyy-MM-dd";
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        // Customize the DateTime format during deserialization
        if (reader.Value != null && reader.Value is string stringValue)
        {
            if (DateTime.TryParseExact(stringValue, DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
        }

        return base.ReadJson(reader, objectType, existingValue, serializer);
    }
}
