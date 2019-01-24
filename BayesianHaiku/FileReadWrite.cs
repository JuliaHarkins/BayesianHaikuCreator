using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianHaiku
{
    class FileReadWrite
    {
        private string _trainingFilePath = "TrainingFiles";
        private string _trainedFilePath = "TrainedBayesianNetworks";
        private string _wordAndSyllableKnowledgeFile = "WordAndSyllableKnowledge.txt";

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
        public void SaveNetworkKnowledge(BayesianNetwork bn)
        {
            //TODO: make this save the file
        }

        public BayesianNetwork LoadNetworkKnowledge()
        {
            BayesianNetwork bn = new BayesianNetwork();
            //TODO: make this load the file.
            return bn;
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

            //grabs all the words and their syllables fromt the file
            for (int i = 1; i < wordPlusSyllableFileContent.Length; i++)
            {
                try
                {
                    string[] wordAndSyllableSplit = wordPlusSyllableFileContent[i].Split('+');
                
                    int syllables = int.Parse(wordAndSyllableSplit[1]);
                    wordSyllablesDictonary.Add(wordAndSyllableSplit[0], syllables);
                }
                catch
                {
                    Console.WriteLine("File WordAndSyllableKnowledge.txt is formatted incorrectly.");
                }
            }
                return wordSyllablesDictonary;
        }
        public void AddWordAndSyllable(string word,int syllable)
        {
            using (StreamWriter sw = File.AppendText(_wordAndSyllableKnowledgeFile))
            {
                sw.WriteLine(word+"+"+syllable);
            }
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
