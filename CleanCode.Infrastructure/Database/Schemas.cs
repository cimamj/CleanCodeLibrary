using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCode.Infrastructure.Database
{//ako je vise schema da je kod uredniji? sto je schema? folder unutar baze u kojem su tablice, view sve
    //po defaultu sve ide pod public schemu u bazi , public.Students
    internal static class Schemas
    {
        public const string Default = "public";
    }
}
