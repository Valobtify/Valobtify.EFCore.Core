namespace Valobtify.EFCore.Core.Shared;

public static class ModelBuilderExtensions
{
    public static IEnumerable<IMutableEntityType> GetEntityTypes(this ModelBuilder modelBuilder)
    {
        return modelBuilder.Model
            .GetEntityTypes()
            .Where(entity => !entity.ClrType.IsSingleValueObject());
    }

    public static object? GetEntityModelBuilder(this ModelBuilder modelBuilder, Type entityType)
    {
        // generate modelBuilder.Entity<Entity>()
        var entityMethod = modelBuilder
            .GetType()
            .GetMethods()
            .Single(m =>
                m is
                {
                    IsGenericMethod: true,
                    IsPublic: true,
                    IsStatic: false,
                    Name: nameof(modelBuilder.Entity)
                } &&
                m.GetParameters().Length == 0)
            .MakeGenericMethod(entityType);
        return entityMethod.Invoke(modelBuilder, null);
    }
    
    public static ModelBuilder ApplyOnSingleValueObjects(
        this ModelBuilder modelBuilder,
        Action<IMutableEntityType, PropertyInfo> action)
    {
        modelBuilder.GetEntityTypes().ToList()
            .ForEach(entity => entity.ClrType
                .GetSingleValueObjectProperties().ToList()
                .ForEach(singleValueObjectProperty => action(entity, singleValueObjectProperty)));
        
        return modelBuilder;
    }
}