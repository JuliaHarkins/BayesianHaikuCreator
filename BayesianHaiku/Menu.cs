using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianHaiku
{
    class Menu
    {
        private BayesianNetwork _bn;
        private FileReadWrite _frw;
        
        public Menu()
        {
            _frw = new FileReadWrite();
        }
        public void DisplayMenu()
        {
            bool displayMenu = true;
            string uInput;
            do
            {
                Console.WriteLine("Please Select an Option or press q to quit");
                Console.WriteLine("1.   Train Network");
                Console.WriteLine("2.   Update Network");
                Console.WriteLine("3.   Generate A haiku");
                Console.WriteLine("q.   quit");
                uInput = Console.ReadLine();

                switch (uInput.ToLower())
                {
                    case "1":
                        _bn = new BayesianNetwork();
                        _bn.Train(_frw.LoadTrainingData());
                        if (_bn.Words != null)
                            SetSylablesCount();
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("\nERROR: No training Data Provided");
                            Console.ReadKey();
                        }
                           
                        break;
                    case "2":
                        
                        break;
                    case "q":
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;

                }
                Console.Clear();
            } while (displayMenu);

        }

        public void SetSylablesCount()
        {
            _bn.SetSylables(_frw.LoadWordAndSyllables());
            int sylable;
            string uInput;
            foreach(Word w in _bn.Words)
            {
                bool invalidInput = true;
                if (w.Sylables == 0 &&  !String.IsNullOrEmpty(w.Name))
                {
                    do
                    {

                        Console.WriteLine("How many sylables does \"" + w.Name + "\" Contaion?");
                        uInput = Console.ReadLine();
                        if (int.TryParse(uInput, out sylable))
                        {
                            invalidInput = false;
                            w.Sylables = sylable;
                            _frw.AddWordAndSyllable(w.Name, sylable);
                        }  
                    } while (invalidInput);
                }
            }
        }
    }
}
