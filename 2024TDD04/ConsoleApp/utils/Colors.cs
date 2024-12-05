using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.utils
{
    public static class Colors
    {
        private static String _reset = "\u001b[0m";
        private static String _black = "\u001b[30m";
        private static String _red = "\u001b[31m";
        private static String _green = "\u001b[32m";
        private static String _yellow = "\u001b[33m";
        private static String _blue = "\u001b[34m";
        private static String _magenta = "\u001b[35m";
        private static String _cyan = "\u001b[36m";
        private static String _white = "\u001b[37m";
        private static String _orange = "\u001b[38;5;208m";

        public static String Reset { get => _reset; }
        public static String Black { get => _black; }
        public static String Red { get => _red; }
        public static String Green { get => _green; }
        public static String Yellow { get => _yellow; }
        public static String Blue { get => _blue; }
        public static String Magenta { get => _magenta; }
        public static String Cyan { get => _cyan; }
        public static String White { get => _white; }
        public static String Orange { get => _orange; }

        public static String Colorize(String text, String color)
        {
            return color + text + _reset;
        }
    }
}
