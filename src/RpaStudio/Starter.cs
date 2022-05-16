using System;

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
