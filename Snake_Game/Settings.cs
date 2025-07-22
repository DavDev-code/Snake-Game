using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Game
{
    internal class Settings
    {
        public static int Width {  get; set; }
        public static int Height { get; set; }

        public static string Directions;

        public Settings()
        {
            Width = 25;
            Height = 25;
            Directions = "left";
        }
    }
}
