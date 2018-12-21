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
        private readonly string _trainingFilePath = "TrainingFiles";
        private readonly string _trainedFilePath = "TrainedBayesianNetworks";

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
        public void SaveNetworkKnowledge(BayesianNetwork bn)
        {
            int num = 1;
            int fileLenght = _trainedFilePath.Count();
            while (File.Exists(_trainedFilePath + "/" + bn.FileName + ".txt"))
            {
                if (num == 1)
                    bn.FileName = bn.FileName + num;
                else
                {
                    List<Char> fileNameChange = bn.FileName.ToCharArray().ToList();

                    for(int i = -1;  i <num.ToString().Count();i++)
                        fileNameChange[fileLenght + i] = (num++).ToString().ToCharArray()[i];
                    bn.FileName = fileNameChange.ToString();
                }
            }
            File.Create(_trainedFilePath + "/" + bn.FileName + ".txt");
            foreach(Word w in bn.Words)
            {

            }
        }
            

        // remove the .txt
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
            foreach (string file in Directory.EnumerateFiles(_trainingFilePath, "*.txt"))
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
