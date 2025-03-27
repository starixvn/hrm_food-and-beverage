using System.ComponentModel;
using System.Reflection;

namespace NexFB.WorkerService.Helper;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        if (fieldInfo == null)
            return value.ToString();

        var attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
        return attribute?.Description ?? value.ToString();
    }
}