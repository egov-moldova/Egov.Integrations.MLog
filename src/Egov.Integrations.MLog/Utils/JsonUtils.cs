using System.Text;

namespace Egov.Integrations.MLog.Utils;

internal static class JsonUtils
{
    public static string EscapeObject(object obj)
    {
        var result = new StringBuilder();
        AppendEscapedJsonObject(result, obj);
        return result.ToString();
    }

    // Using our own escaping to not depend on a JSON library
    public static void AppendEscapedJsonObject(StringBuilder result, object? obj)
    {
        switch (obj)
        {
            case null:
                result.Append("null");
                break;
            case bool boolValue:
                result.Append(boolValue);
                break;
            case byte byteValue:
                result.Append(byteValue);
                break;
            case sbyte sbyteValue:
                result.Append(sbyteValue);
                break;
            case char charValue:
                result.Append(charValue);
                break;
            case decimal decimalValue:
                result.Append(decimalValue);
                break;
            case double doubleValue:
                result.Append(doubleValue);
                break;
            case float floatValue:
                result.Append(floatValue);
                break;
            case int intValue:
                result.Append(intValue);
                break;
            case uint uintValue:
                result.Append(uintValue);
                break;
            case long longValue:
                result.Append(longValue);
                break;
            case ulong ulongValue:
                result.Append(ulongValue);
                break;
            case short shortValue:
                result.Append(shortValue);
                break;
            case ushort ushortValue:
                result.Append(ushortValue);
                break;
            default:
                var value = obj.ToString();
                if (value == null)
                {
                    result.Append("null");
                    break;
                }

                result.Append('"');
                var length = value.Length;
                for (var i = 0; i < length; i++)
                {
                    var ch = value[i];
                    switch (ch)
                    {
                        case '\\':
                        case '"':
                        case '/':
                            result.Append('\\');
                            result.Append(ch);
                            break;
                        case '\b':
                            result.Append("\\b");
                            break;
                        case '\f':
                            result.Append("\\f");
                            break;
                        case '\n':
                            result.Append("\\n");
                            break;
                        case '\r':
                            result.Append("\\r");
                            break;
                        case '\t':
                            result.Append("\\t");
                            break;
                        default:
                            if (ch < ' ')
                            {
                                result.Append("\\u" + ((int)ch).ToString("x4"));
                            }
                            else
                            {
                                result.Append(ch);
                            }

                            break;
                    }
                }

                result.Append('"');
                break;
        }
    }
}