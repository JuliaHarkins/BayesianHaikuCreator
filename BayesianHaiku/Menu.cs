using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianHaiku
{
    class Menu
    {
        /// <summary>
        /// The AI that's currently in use
        /// </summary>
        private BayesianNetwork _bn;
        /// <summary>
        /// The object used to save and retrieve information about the network
        /// </summary>
        private FileReadWrite _frw;
        /// <summary>
        /// Instanciates the member variables and allows
        /// </summary>
        public Menu()
        {
            _frw = new FileReadWrite();
            _bn = new BayesianNetwork();
        }
        /// <summary>
        /// The main menu for the program
        /// </summary>
        public void DisplayMenu()
        {
            bool displayMenu = true;
            string uInput;
            do
            {
                Console.WriteLine("Please Select an Option or press q to quit");
                Console.WriteLine("1.   Train Network");
                Console.WriteLine("2.   Generate A haiku");
                Console.WriteLine("q.   quit");
                uInput = Console.ReadLine();

                switch (uInput.ToLower())
                {
                    case "1":
                        _bn.Train(_frw.LoadTrainingData());
                        if (_bn.Words == null || _bn.Words.Count==0)
                        {
                            Console.Clear();
                            Console.WriteLine("\nERROR: No training Data Provided");
                            Console.ReadKey();
                        }
                        SaveNetwork();
                        break;
                    case "2":
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
                            List<string[]> haiku =_bn.CreateHaiku();

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
        /// <summary>
        /// Asks the user for a valid file name and
        /// saves the file if it's not already in existance
        /// </summary>
        private void SaveNetwork()
        {
            bool validFileName = false;
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
                            saveFile = false;
                            invalidInput = false;
                            break;
                        case "n":
                            saveFile = true;
                            invalidInput = false;
                            break;
                        default:
                            invalidInput = true;
                            break;
                    }
                }
            } while (invalidInput);


            while (!validFileName && saveFile)
            {
                Console.WriteLine("Please name the AI");
                _bn.FileName = Console.ReadLine();
                validFileName = _frw.SaveNetworkKnowledge(_bn);
            } 
        }
    }
}
