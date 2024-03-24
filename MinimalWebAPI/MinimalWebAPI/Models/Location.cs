using System.Globalization;
using System.Reflection;

namespace MinimalWebAPI.Models;

public class Location
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    // We can use also
    // public static bool TryParse(string? value, out Location? location)
    // if we don't need the FormatProvider
    public static bool TryParse(string? value, IFormatProvider? provider, out Location? location)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            var values = value.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 2
                && double.TryParse(values[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var latitude)
                && double.TryParse(values[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var longitude))
            {
                location = new Location { Latitude = latitude, Longitude = longitude };
                return true;
            }
        }

        location = null;
        return false;
    }

    // We can use also
    // public static bool BindAsync(HttpContext content)
    // if we don't need the ParameterInfo
    public static ValueTask<Location?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (double.TryParse(context.Request.Query["lat"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var latitude)
            && double.TryParse(context.Request.Query["lon"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var longitude))
        {
            var location = new Location { Latitude = latitude, Longitude = longitude };
            return ValueTask.FromResult<Location?>(location);
        }

        return ValueTask.FromResult<Location?>(null);
    }
}