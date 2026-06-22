using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skladiste
{
    public class Repository
    {
        private static Repository _instance;
        private static object locker = new object();

        private Repository()
        {
            Zakupi = new List<Zakup>();
        }

        public static Repository Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null)
                    {
                        _instance = new Repository(); 
                    }
                }
                return _instance;
            }
        }

        public IList<Zakup> Zakupi { get; set; }
    }
}
