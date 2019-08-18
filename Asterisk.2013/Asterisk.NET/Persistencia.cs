using System;
using System.Collections.Generic;

namespace AsterNET
{
    public static class Persistencia
    {
        static Persistencia()
        {
            FilaPraLigar = new List<string>();
        }

        public static List<string> FilaPraLigar {get; set;}
    }
}
