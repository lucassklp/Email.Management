using System.Collections.Generic;

namespace Email.Management.Dtos
{
    public class PagedResultDto<TResult>
    {
        public long Total { get; set; }
        public List<TResult> Content { get; set; }
    }
}
