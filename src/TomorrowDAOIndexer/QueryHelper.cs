using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using AeFinder.Sdk.Entities;
using AElf.CSharp.Core;

namespace TomorrowDAOIndexer;

public class QueryHelper
{
    public static IQueryable<T> AddEqualFilter<T>(IQueryable<T> queryable, string propertyPath, object value)
    {
        var parameter = Expression.Parameter(typeof(T), "o");
        var paths = propertyPath.Split('.');
        var expression = paths.Aggregate<string?, Expression>(parameter, Expression.Property!);
        Expression constant = Expression.Constant(Convert.ChangeType(value, expression.Type), expression.Type);
        Expression equal = Expression.Equal(expression, constant);
        return queryable.Where(Expression.Lambda<Func<T, bool>>(equal, parameter));
    }

    public static List<T> GetAllIndex<T>(IQueryable<T> queryable) where T : AeFinderEntity
    {
        const int maxSize = 10000;
        queryable = queryable.OrderBy(a => a.Metadata.Block.BlockHeight).ThenBy(a => a.Id).Take(maxSize);
        var list = queryable.ToList();
        while (list.Count != 0 && list.Count % maxSize == 0)
        {
            var blocks = queryable.After([list.Last().Metadata.Block.BlockHeight, list.Last().Id]).ToList();
            if (blocks.Count == 0)
            {
                break;
            }
            list.AddRange(blocks);
        }

        return list;
    }
}