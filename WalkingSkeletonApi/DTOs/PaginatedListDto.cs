using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.DTOs
{
    public class PaginatedListDto<T>
    {
        public PageMeta MetaData { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PaginatedListDto()
        {
            Data = new List<T>();
        }
    }
}
