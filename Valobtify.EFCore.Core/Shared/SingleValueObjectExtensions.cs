namespace Valobtify.EFCore.Core.Shared;

internal static class SingleValueObjectExtensions
{
    public static IEnumerable<PropertyInfo> GetSingleValueObjectProperties(this Type type)
    {
        return type
            .GetProperties()
            .Where(property => property.PropertyType.IsSingleValueObject());
    }
}