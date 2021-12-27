using System.Collections.Generic;

namespace Domain.Responses
{
    public class PaginatedResponse<T> where T : class
    {
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public IEnumerable<T> Elements { get; set; }
    }
}