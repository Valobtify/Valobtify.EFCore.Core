using Valobtify.EFCore.Core.Abstraction;

namespace Valobtify.EFCore.Core;

public static class Configurator
{
    public static ModelBuilder SetupOwnedSingleValueObjects<TOwnedSingleValueObjectConfigurator>(
        ModelBuilder modelBuilder,
        Action<SingleValueObjectConfig>? configurationAction = null)
        where TOwnedSingleValueObjectConfigurator : IOwnedSingleValueObjectConfigurator
    {
        var singleValueObjectsConfig = new SingleValueObjectConfig { ModelBuilder = modelBuilder };

        configurationAction?.Invoke(singleValueObjectsConfig);

        modelBuilder.ApplyOnSingleValueObjects((entity, singleValueObjectProperty) =>
        {
            // ModelBuilder<TEntity>
            var entityModelBuilder = modelBuilder.GetEntityModelBuilder(entity.ClrType);

            var singleValueObjectValueType = singleValueObjectProperty.PropertyType
                .GetProperty(nameof(Template.Value))!.PropertyType;

            var setupSingleValueObjectMethod =
            (typeof(TOwnedSingleValueObjectConfigurator)
            .GetMethod(nameof(IOwnedSingleValueObjectConfigurator.ConfigureOwnedSingleValueObject)) ??
            throw new InvalidOperationException("method is null"))
            .MakeGenericMethod(entity.ClrType, singleValueObjectProperty.PropertyType,
            singleValueObjectValueType);

            var expression = Utilities.CreateExpression(entity, singleValueObjectProperty);

            var maxlength = singleValueObjectsConfig.GetMaxLength(singleValueObjectProperty.PropertyType);

            setupSingleValueObjectMethod.Invoke(null, [entityModelBuilder, expression, maxlength]);
        });

        return modelBuilder;
    }
}