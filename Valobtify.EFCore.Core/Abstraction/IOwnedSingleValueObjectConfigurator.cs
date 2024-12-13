namespace Valobtify.EFCore.Core.Abstraction;

public interface IOwnedSingleValueObjectConfigurator
{
    public static abstract EntityTypeBuilder<TEntity> ConfigureOwnedSingleValueObject<TEntity, TRelatedEntity,
        TSingleValueObjectType>(EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TRelatedEntity?>> navigationExpression, int? maxLength = null)
        where TEntity : class
        where TRelatedEntity : SingleValueObject<TRelatedEntity, TSingleValueObjectType>,
        ICreatableValueObject<TRelatedEntity, TSingleValueObjectType>
        where TSingleValueObjectType : notnull;
}