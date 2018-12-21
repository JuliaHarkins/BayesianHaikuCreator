using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianHaiku
{
    class BayesianNetwork
    {
        private string _fileLocation;
        private List<Word> _words;

        public List<Word> Words { get { return _words; } set { _words = value; } }
        public string FileName { get { return _fileLocation; } set { _fileLocation = value; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="corpus">The words in the training data</param>
        public void Train(string[] corpus)
        {
            bool exists;

            for(int i =0; i< corpus.Count(); i++) { 
                exists = false;
                foreach(Word w in _words)
                {
                    if(w.Name == corpus[i])
                    {
                        exists = true;
                        w.AppearanceCount++;

                        
                        if (i != corpus.Count() - 1)
                        {
                            if (!w.SubsequentWords.ContainsKey(corpus[i + 1]))
                                w.SubsequentWords.Add(corpus[i + 1], 1);
                            else
                                w.SubsequentWords[corpus[i + 1]]++;
                        }
                    }
                }
                if (!exists)
                {
                    Word newWord = new Word
                    {
                        Name = corpus[i],
                        AppearanceCount = 1,
                        Sylables = 0
                    };
                    if (i != corpus.Count() - 1)
                        newWord.SubsequentWords.Add(corpus[i + 1], 1);

                }
            }
        }

    }
}
