using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianHaiku
{
    class Program
    {
        /// <summary>
        /// the main access point to the program
        /// </summary>
        static void Main(string[] args)
        {
            Menu m = new Menu();
            m.DisplayMenu();
        }
    }
}
