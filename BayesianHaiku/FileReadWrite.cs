using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BayesianHaiku
{
    using Newtonsoft.Json;
    using System.Diagnostics;

    class FileReadWrite
    {
        /// <summary>
        /// The Path that contains all the trainign data
        /// </summary>
        private readonly string _trainingFilePath = "TrainingFiles";
        /// <summary>
        /// The path in which the AI is saved
        /// </summary>
        private readonly string _trainedFilePath = "TrainedBayesianNetworks";

        /// <summary>
        /// Constructor for the class
        /// </summary>
        public FileReadWrite()
        {
            if (!Directory.Exists(_trainingFilePath))
            {
                Directory.CreateDirectory(_trainingFilePath);
            }
            if (!Directory.Exists(_trainedFilePath))
            {
                Directory.CreateDirectory(_trainedFilePath);
            }

        }
        /// <summary>
        /// saves the given network to a txt file
        /// </summary>
        /// <param name="bn">the network to be saved</param>
        /// <returns></returns>
        public bool SaveNetworkKnowledge(BayesianNetwork bn)
        {
            string path;
            bool fileSaved = false;

            //checks if the file type is included
            if (!bn.FileName.EndsWith(".txt"))
                path = _trainedFilePath + "/" + bn.FileName + ".txt";
            else
                path = _trainedFilePath + "/" + bn.FileName;

            try
            {
                if (!File.Exists(path))

                {
                    //converts the object to be placed into a file
                    string bnJsonString = JsonConvert.SerializeObject(bn);

                    //creates the file
                    FileStream s = File.Create(path);
                    s.Close();

                    //saves the bayesian network to the file
                    StreamWriter sw = File.AppendText(path);
                    sw.WriteLine(bnJsonString);
                    sw.Close();

                    fileSaved = true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error : " + e);
            }
            return fileSaved;
        }
        /// <summary>
        /// Gets a network based on the given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public BayesianNetwork LoadExistingNetwork(string path)
        {
            BayesianNetwork bn = new BayesianNetwork();

            //gets the object in its text format from the file
            string bnJsonString = File.ReadAllText(path);
            //converts the text back to the object
            bn = JsonConvert.DeserializeObject<BayesianNetwork>(bnJsonString);

            return bn;
        }

        /// <summary>
        /// Gets the names of all the saved AI files
        /// </summary>
        public string[] TrainedBayesianNetworkNames()
        {
            string[] networkNames = Directory.GetFiles(_trainedFilePath, "*.txt");

            return networkNames;
        }
        /// <summary>
        /// Reads the information from the files in the training data
        /// folder, then removes the punctuation from it and splits it
        /// into indevidual words.
        /// </summary>
        /// <returns>the words from the body of text</returns>
        public string[] LoadTrainingData()
        {
            List<string> words = new List<string>();
            foreach (string file in Directory.EnumerateFiles(_trainingFilePath, "*.txt"))
            {
                string trainingData = File.ReadAllText(file).ToLower();
                //gets rid of the punctuation
                string withoutPunctuation = RemovePuntuation(trainingData);
                //adds the individual words
                foreach (string w in withoutPunctuation.Split())
                {
                    words.Add(w);
                }
            }

            return words.ToArray(); ;
        }

        /// <summary>
        /// removes the puctuation form a body of text
        /// </summary>
        /// <param name="corpus">a body of text</param>
        /// <returns>reurns the body of text without the puntuation</returns>
        private string RemovePuntuation(string corpus)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in corpus)
            {
                if (!char.IsPunctuation(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
