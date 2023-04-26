using System.Collections.Generic;

namespace VLUTUTORS.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public IReadOnlyCollection<T> Items { get; set; }
    }
}
