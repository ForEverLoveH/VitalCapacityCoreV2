using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitalCapacityV2.Summer.GameSystem
{
    public class Singleton<T> where T : new()
    {
        /// <summary>
        ///
        /// </summary>
        private static T instance;

        /// <summary>
        ///
        /// </summary>
        private static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();
                return instance;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static T GetInstance()
        {
            return Instance;
        }
    }
}