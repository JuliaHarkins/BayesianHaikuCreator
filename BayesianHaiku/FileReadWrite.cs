using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using 
using System.Threading.Tasks;

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
        public bool SaveNetworkKnowledge(BayesianNetwork bn)
        {
            bool fileSaved = false;
            string path = _trainedFilePath + "/" + bn.FileName + ".txt";
            //string subWords;

            try
            {
                if (!File.Exists(path))

                {
                    FileStream s = File.Create(path);
                    
                    s.Close();

                    StreamWriter sw = File.AppendText(path);

                    //sw.WriteLine(bn.TotalWords);
                    string bnJsonString = JsonConvert.SerializeObject(bn);
                    sw.WriteLine(bnJsonString);
                    //foreach (Word w in bn.Words)
                    //{
                    //    sw.Write(w.Name + "," + w.Sylables + "," + w.AppearanceCount + ",");
                    //    subWords = "";
                    //    foreach (KeyValuePair<string, int> kvp in w.SubsequentWords)
                    //    {
                    //        subWords = subWords + kvp.Key + "+" + kvp.Value + ";";
                    //    }
                    //    sw.Write(subWords.Remove(subWords.Length - 1) + "=");
                    //}
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in saving the file. " + e);
                
            }
            return fileSaved;
        }
        public BayesianNetwork LoadExistingNetwork(string path)
        {
            BayesianNetwork bn = new BayesianNetwork();
            //StreamReader sr = new StreamReader(path);
            string bnJsonString = File.ReadAllText(path);   //sr.ReadToEnd();
            bn = JsonConvert.DeserializeObject<BayesianNetwork>(bnJsonString);

            StringBuilder sb = new StringBuilder();
            

            //string bnJsonString = sr.ReadToEnd(path);
            //string[] wordSplitNetwork = textNetwork.Split('=');
            //bn.TotalWords = int.TryParse(wordSplitNetwork[0], out int totalwords) == true ? totalwords: 0;
            //for (int i = 1; i < wordSplitNetwork.Count(); i++)
            //{
            //    string[] wordContentSplitNetwork = textNetwork.Split(',');
            //    //foreach (string word = wordContentSplitNetwork);
            //    //{
            //     //   bn.
            //    //}
            //}


            return bn;
        }
            

        // remove the .txt
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
