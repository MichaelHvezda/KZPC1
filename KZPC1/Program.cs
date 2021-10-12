using MonoGame.Extended.VideoPlayback;
using System;

namespace KZPC1
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            FFmpegBinariesHelper.InitializeFFmpeg("x86", "x64");

            using (var game = new Game1())
                game.Run();
        }
    }
}
