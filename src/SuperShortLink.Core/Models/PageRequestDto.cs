namespace SuperShortLink.Models
{
    /// <summary>
    /// 请求分页模型
    /// </summary>
    public class PageRequestDto
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int page_index { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int page_size { get; set; } = 20;
    }
}
