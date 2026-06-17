using UnityEngine;

public static class EffectsUtils
{
    public static void InvalidParameters(int position, string expectedType)
    {
        Debug.LogError($"Invalid parameter at position {position}. Expected type: {expectedType}");
    }

    public static bool TryParseFloat(string value, out float result)
    {
        return float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
    }
}