using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUtils
{
    public static class TaskHelpers
    {
        public static void RunSynchronous(this Task task)
        {
            Task.Run(() => task).Wait();
        }

        public static T RunSynchronous<T>(this Task<T> task)
        {
            return Task.Run(() => task).Result;
        }
    }
}
