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
        private string _TrainingFilePath = "TrainingFiles";
        private string _TrainedFilePath = "TrainedBayesianNetworks";

        public FileReadWrite()
        {
            if (!Directory.Exists(_TrainingFilePath))
            {
                Directory.CreateDirectory(_TrainingFilePath);
            }
            if (!Directory.Exists(_TrainedFilePath))
            {
                Directory.CreateDirectory(_TrainedFilePath);
            }

        }
        public void SaveNetworkKnowledge(BayesianNetwork bn)
        {

        }

        public BayesianNetwork LoadNetworkKnowledge()
        {
            BayesianNetwork bn = new BayesianNetwork();

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
            foreach (string file in Directory.EnumerateFiles(_TrainingFilePath, "*.txt"))
            {
                //gets rid of the punctuation
                string withoutPunctuation = RemovePuntuation(file);
                //adds ther individual words
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
