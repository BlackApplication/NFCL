using System.Reflection;

namespace Services.Helper;

public static class ObjectHelper {
    public static void UpdateProperties<T>(T target, T source) {
        if (target == null || source == null)
            throw new ArgumentNullException("Target or source object is null");

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties) {
            if (property.CanWrite) {
                var value = property.GetValue(source);
                property.SetValue(target, value);
            }
        }
    }
}
