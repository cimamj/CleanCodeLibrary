using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeLibrary.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        int? GetStudentId();
        string GetRole();
        bool IsAuthenticated();

        bool IsAdmin();
    }
}
