using System.ComponentModel.DataAnnotations;

namespace Valobtify.EFCore.Core;

public class SingleValueObjectConfig
{
    public readonly Dictionary<Type, int?> SingleValueObjectsMaxLength = [];
    public required ModelBuilder ModelBuilder { get; init; }

    public SingleValueObjectConfig SetupMaxlength<TSingleValueObject>(
        int maxLength)
        where TSingleValueObject : SingleValueObject<TSingleValueObject, string>,
        ICreatableValueObject<TSingleValueObject, string>
    {
        SingleValueObjectsMaxLength.Add(typeof(TSingleValueObject), maxLength);

        return this;
    }

    public int? GetMaxLength(Type singleValueObjectType)
    {
        var maxlength = singleValueObjectType
            .GetProperty(nameof(Template.Value))!
            .GetCustomAttribute<MaxLengthAttribute>()?.Length;

        if (maxlength is null)
            SingleValueObjectsMaxLength.TryGetValue(singleValueObjectType, out maxlength);

        return maxlength;
    }
}