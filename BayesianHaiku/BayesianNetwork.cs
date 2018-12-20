using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianHaiku
{
    class BayesianNetwork
    {
        private List<Word> _words;
        public List<Word> Words { get { return _words; } set { _words = value; } }

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

                        if (i != 0)
                        {
                            if (!w.PriorWords.ContainsKey(corpus[i - 1]))
                                w.PriorWords.Add(corpus[i - 1], 1);
                            else
                                w.PriorWords[corpus[i - 1]]++;
                        }
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
                    Word newWord = new Word();
                    newWord.Name = corpus[i];
                    newWord.AppearanceCount = 1;
                    newWord.Sylables = 0;

                    if (i != 0)
                        newWord.PriorWords.Add(corpus[i - 1], 1);

                    if (i != corpus.Count() - 1)
                        newWord.SubsequentWords.Add(corpus[i + 1], 1);

                }
            }
        }

    }
}
