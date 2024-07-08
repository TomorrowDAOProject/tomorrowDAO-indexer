namespace TomorrowDAOIndexer;

public class QueryHelper
{
    // public static IQueryable<T> AddEqualFilter<T>(IQueryable<T> queryable, string propertyPath, object value)
    // {
    //     var parameter = Expression.Parameter(typeof(T), "o");
    //     var paths = propertyPath.Split('.');
    //     var expression = paths.Aggregate<string?, Expression>(parameter, Expression.Property!);
    //     Expression constant = Expression.Constant(Convert.ChangeType(value, expression.Type), expression.Type);
    //     Expression equal = Expression.Equal(expression, constant);
    //     return queryable.Where(Expression.Lambda<Func<T, bool>>(equal, parameter));
    // }
}