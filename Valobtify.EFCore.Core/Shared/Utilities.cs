namespace Valobtify.EFCore.Core.Shared;

internal static class Utilities
{
    public static object? CreateExpression(IMutableEntityType entity, PropertyInfo singleValueObjectProperty)
    {
        // crete expression
        var parameter = Expression.Parameter(entity.ClrType, "entity");
        var propertyAccess = Expression.Property(parameter, singleValueObjectProperty);

        //create Expression.Lambda<Func<TEntity,TSingleValueProperty>>
        var lambdaMethod = typeof(Expression)
            .GetMethods()
            .Single(method =>
                method is
                {
                    Name: nameof(Expression.Lambda),
                    IsGenericMethod: true
                } &&
                method.GetGenericArguments().Length is 1 &&
                method.GetGenericArguments().First().Name is "TDelegate" &&
                method.GetParameters().Length is 2 &&
                method.GetParameters()[1].ParameterType == typeof(ParameterExpression[]));

        var funcType = typeof(Func<,>)
            .MakeGenericType(entity.ClrType, singleValueObjectProperty.PropertyType);

        ParameterExpression[] parameters = [parameter];

        return lambdaMethod
            .MakeGenericMethod(funcType)
            .Invoke(null, [propertyAccess, parameters]);
    }
}