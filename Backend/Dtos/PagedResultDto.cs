namespace Email.Management.Dtos
{
    public class PagedResultDto<TResult>
    {
        public long Total { get; set; }
        public TResult Content { get; set; }
    }
}
