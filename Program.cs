using System;

namespace C16_Ex03_Yakir_201049475_Omer_300471430
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new Invaders())
            {
                game.Run();
            }
        }
    }
#endif
}
