using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpaStudio
{
    public static class Starter
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new App().Run();
        }
    }
}
