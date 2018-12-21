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
                        SetSylablesCount();
                        CheckSave();
                        break;
                    case "2":
                        break;
                    case "q":
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        break;

                }
                
            } while (displayMenu);
        }

        public void SetSylablesCount()
        {
            string uInput;
            foreach(Word w in _bn.Words)
            {
                bool invalidInput = true;
                if (w.Sylables == 0)
                {
                    Console.Clear();
                    do
                    {
                        Console.WriteLine("How many sylables does \"" + w.Name + "\" contaion?");
                        uInput = Console.ReadLine();
                        if (int.TryParse(uInput, out int sylable) && sylable !=0)
                        {
                            invalidInput = false;
                            w.Sylables = sylable;
                        }
                        else
                        {
                            Console.WriteLine("invalid amount");
                        }
                    } while (invalidInput);
                }
            }
        }

        public void CheckSave()
        {
            Console.WriteLine("Would you like to save the network?");
            Console.WriteLine("enter y to save or anythin else to cancel");
            string uInput = Console.ReadLine();
            bool save = uInput.ToLower() == "y" ? true : false;

            do
            {
                if (save)
                {
                    do
                    {
                        if (_bn.FileName == "")
                        {
                            Console.WriteLine("What would you like to name this file?");
                            _bn.FileName = Console.ReadLine();
                        }
                    } while (_bn.FileName == "");
                    save = false;
                    _frw.SaveNetworkKnowledge(_bn);
                }
                else
                {
                    Console.WriteLine("Are you sure you don't want to save?");
                    Console.WriteLine("enter y to save or anything else to cancel");
                    uInput = Console.ReadLine();
                    save = uInput.ToLower() == "y" ? true : false;
                }

            } while (save);
        }
    }
}
