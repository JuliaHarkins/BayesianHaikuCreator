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
        private readonly string _trainingFilePath = "TrainingFiles";
        private readonly string _trainedFilePath = "TrainedBayesianNetworks";
        private readonly string _wordAndSyllableKnowledgeFile = "WordAndSyllableKnowledge.txt";

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
            if (!File.Exists(_wordAndSyllableKnowledgeFile))
            {
                File.Create(_wordAndSyllableKnowledgeFile);
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
        /// gets a network based on the given path
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

        public string[] TrainedBayesianNetworkNames()
        {
            string[] networkNames = Directory.GetFiles(_trainedFilePath, "*.txt");

            return networkNames;
        }
        /// <summary>
        /// reads the information from the files in the training data
        /// folder, then removes the punctuation from it and splits it
        /// into indevidual words.
        /// </summary>
        /// <returns>the words from the body of text</returns>
        public string[]  LoadTrainingData()
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
        public Dictionary<string, int> LoadWordAndSyllables()
        {
            //the Dictionary of all known words and their syllables 
            Dictionary<string, int> wordSyllablesDictonary = new Dictionary<string, int>();

            //the file contents
            string[] wordPlusSyllableFileContent = File.ReadAllLines(_wordAndSyllableKnowledgeFile);
            string s ="";
            string word= "";
            //grabs all the words and their syllables fromt the file
            for (int i = 1; i < wordPlusSyllableFileContent.Length; i++)
            {
                try
                {
                    string[] wordAndSyllableSplit = wordPlusSyllableFileContent[i].Split('+');
                     s = wordAndSyllableSplit[1];
                     word = wordAndSyllableSplit[0];
                    int syllables = int.Parse(s);
                    wordSyllablesDictonary.Add(word, syllables);
                }
                catch
                {
                    Console.WriteLine("File WordAndSyllableKnowledge.txt is formatted incorrectly. at:"+ word+"+"+s);
                }
            }
                return wordSyllablesDictonary;
        }

        public void AddWordAndSyllable(string word,int syllable)
        {
            StreamWriter sw = new StreamWriter(_wordAndSyllableKnowledgeFile);

            sw.WriteLine(word + "+" + syllable);
            sw.Close();

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
