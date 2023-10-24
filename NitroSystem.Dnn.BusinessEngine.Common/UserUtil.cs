using DotNetNuke.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Common
{
    public static class UserUtil
    {
        public static string GetUserDisplayName(int userID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                return ctx.ExecuteScalar<string>(System.Data.CommandType.Text, "Select DisplayName From dbo.Users Where UserID = @0", userID);
            }
        }
    }
}
