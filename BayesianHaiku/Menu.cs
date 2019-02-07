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
            _bn = new BayesianNetwork();
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
                        _bn.Train(_frw.LoadTrainingData());
                        //if (_bn.Words != null || _bn.Words.Count==0)
                            //SetSylablesCount();
                        //else
                        //{
                            Console.Clear();
                            Console.WriteLine("\nERROR: No training Data Provided");
                            Console.ReadKey();
                        //}
                        SaveNetwork();
                        break;

                    case "2":
                        break;

                    case "3":
                        Console.Clear();

                        string[] availableNetworks =_frw.TrainedBayesianNetworkNames();
                        int networkNum=-100;

                        if (availableNetworks.Count() != 0)
                        {

                            //list the already saved networks
                            Console.WriteLine("Select the Network you'd like to use");
                            for (int i = 1; i <= availableNetworks.Count(); i++)
                                Console.WriteLine(i + ".    " + availableNetworks[i - 1]);

                            bool outOfBounds = true;
                            //validate the userinput
                            do
                            {
                                uInput = Console.ReadLine();
                                int.TryParse(uInput, out networkNum);

                                if ((networkNum > 0) && (networkNum <= availableNetworks.Count()))
                                    outOfBounds = false;
                                else
                                    Console.WriteLine("invalid input");

                            } while(outOfBounds);

                            //setting the network
                            _bn = _frw.LoadExistingNetwork(availableNetworks[networkNum-1]);
                            List<string[]> haiku =_bn.HaikuCreator();

                            Console.Clear();
                            Console.WriteLine("Press any key to exit. You're haiku is:");
                            foreach(string[] line in haiku)
                            {
                                Console.WriteLine();
                                foreach(string word in line)
                                {
                                    Console.Write(word +" ");
                                }
                            }
                            Console.ReadKey();
                           
                        }
                        else
                        {
                            Console.WriteLine("ERROR: No trained networks available.");
                            Console.ReadKey();
                        }
                        break;

                    case "q":
                        displayMenu = false;
                        break;

                    default:
                        Console.WriteLine("Invalid menu option");
                        break;

                }
                Console.Clear();
            } while (displayMenu);

        }
        private void SetSylablesCount()
        {
            _bn.SetSylables(_frw.LoadWordAndSyllables());
            int sylable;
            string uInput;
            foreach(Word w in _bn.Words)
            {
                bool invalidInput = true;
                if (w.Syllables == 0)
                {
                    do
                    {

                        Console.WriteLine("How many sylables does \"" + w.Name + "\" Contaion?");
                        uInput = Console.ReadLine();
                        if (int.TryParse(uInput, out sylable))
                        {
                            invalidInput = false;
                            w.Syllables = sylable;
                            _frw.AddWordAndSyllable(w.Name, sylable);
                        }  
                    } while (invalidInput);
                }
            }
        }
        private void SaveNetwork()
        {
            bool invalidFileName = true;
            bool saveFile = true;
            bool invalidInput = true;
            string userInput;
            do
            {
                Console.WriteLine("would you like to save the AI? y/n");
                userInput = Console.ReadLine().ToLower();
                switch (userInput)
                {
                    case "y":
                        saveFile = true;
                        invalidInput = false;
                        break;
                    case "n":
                        saveFile = false;
                        invalidInput = false;
                        break;
                        default:
                        invalidInput = true;
                        break;
                }
                if(saveFile == false)
                {
                    Console.WriteLine("Are you sure you don't want to save the AI? y/n");
                    userInput = Console.ReadLine().ToLower();
                    switch (userInput)
                    {
                        case "y":
                            saveFile = true;
                            invalidInput = false;
                            break;
                        case "n":
                            saveFile = false;
                            invalidInput = false;
                            break;
                        default:
                            invalidInput = true;
                            break;
                    }
                }
            } while (invalidInput);


            while (invalidFileName && saveFile)
            {
                Console.WriteLine("Please name the AI");
                _bn.FileName = Console.ReadLine();
                invalidFileName = _frw.SaveNetworkKnowledge(_bn);
            } 
        }
    }
}
