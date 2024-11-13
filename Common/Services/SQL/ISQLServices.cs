using Common.Models.Data;
using Common.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.SQL
{
    public interface ISQLServices
    {
        ListChildrenGuardianView GetListOfAllChildrenAndGuardians();

    }

}
