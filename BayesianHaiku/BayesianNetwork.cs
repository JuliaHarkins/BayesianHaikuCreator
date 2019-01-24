using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianHaiku
{
    class BayesianNetwork
    {
        private int _totalWords;
        private List<Word> _words;
        private string _fileName;

        public List<Word> Words { get { return _words; } set { _words = value; } }
        public string FileName { get { return _fileName; } set { _fileName = value; } }

        public BayesianNetwork()
        {
            _totalWords = 0;
            _words = new List<Word>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="corpus">The words in the training data</param>
        public void Train(string[] corpus)
        {
            bool alreadyExistingWord;

            for(int i =0; i< corpus.Count(); i++) {
                _totalWords++;
                alreadyExistingWord = false;
                if (_words != null)
                foreach(Word w in _words)
                {
                    if(w.Name == corpus[i])
                    {
                        alreadyExistingWord = true;
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
                if (!alreadyExistingWord)
                {

                    Word newWord = new Word();
                    newWord.Name = corpus[i];

                    if (i != corpus.Count() - 1 && corpus.Count() != 0)
                        newWord.SubsequentWords.Add(corpus[i + 1], 1);
                    _words.Add(newWord);

                }
            }
        }
        public void SetSylables(Dictionary<string, int> wordAndSyllables)
        {
            //if(_words!=null)
            foreach(Word w in _words)
            {
                if (wordAndSyllables.ContainsKey(w.Name))
                    w.Sylables = wordAndSyllables[w.Name];
            }
        }

    }
}
