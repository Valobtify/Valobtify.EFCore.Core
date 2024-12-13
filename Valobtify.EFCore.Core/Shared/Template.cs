namespace Valobtify.EFCore.Core.Shared;

internal sealed class Template : SingleValueObject<Template, string>, ICreatableValueObject<Template, string>
{
    private Template(string value) : base(value)
    {
    }

    public static Result<Template> Create(string value)
    {
        return new Template(value);
    }
}