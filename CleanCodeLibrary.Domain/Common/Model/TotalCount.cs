using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeLibrary.Domain.Common.Model
{
    public class TotalCount
    {
        public int value { get; set; }
        public TotalCount(int totalCount)
            { value = totalCount; }

        public TotalCount() { }
    }
}
