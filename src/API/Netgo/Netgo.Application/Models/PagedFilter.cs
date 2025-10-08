using System.Linq.Expressions;

namespace Netgo.Application.Models
{
    public class PagedFilter<T>
    {
        public Expression<Func<T, bool>>? Filter { get; set; } = null;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}
