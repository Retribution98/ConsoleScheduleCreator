using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleScheduleCreator
{
    public static class Utils
    {
        public static Guid ToGuid(this int id)
        {
            return new Guid(id, 0, 0, new byte[] { 0,0,0,0,0,0,0,0});
        }
    }
}
