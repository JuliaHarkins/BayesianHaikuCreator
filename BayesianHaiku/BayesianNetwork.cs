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
        public int TotalWords { get { return _totalWords; } set { _totalWords = value; } }

        public BayesianNetwork()
        {
            _totalWords = 0;
            _words = new List<Word>();
        }
        /// <summary>
        /// takes a list of words and turns them into something that makes sense to the network
        /// </summary>
        /// <param name="corpus">The words in the training data</param>
        public void Train(string[] corpus)
        {
            bool alreadyExistingWord;

            for (int i = 0; i < corpus.Count(); i++) {
                if (!String.IsNullOrEmpty(corpus[i])) {
                    _totalWords++;
                    alreadyExistingWord = false;
                    if (_words != null)
                        foreach (Word w in _words)
                        {
                            if (w.Name == corpus[i])
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

                        Word newWord = new Word
                        {
                            Name = corpus[i]
                        };

                        if (i != corpus.Count() - 1 && corpus.Count() != 0)
                            newWord.SubsequentWords.Add(corpus[i + 1], 1);
                        _words.Add(newWord);

                    }
                }
            }
        }
        public List<string[]> HaikuCreator()
        {
            Random rng = new Random();
            string start = _words[rng.Next(_words.Count() - 1)].Name;
            List<string[]> haiku = new List<string[]>();

            string[] lineOne = BuildLine(5, start);
            

            haiku.Add(lineOne);
            string[] lineTwo = BuildLine(7, lineOne[lineOne.Count()-1]);
            haiku.Add(lineTwo);
            string[] lineThree = BuildLine(5, lineTwo[lineTwo.Count() - 1]); 
            haiku.Add(lineThree);

            return haiku;
        }
        private string[] BuildLine(int sylablyesAllowed, string firstString)
        {
            Word word;
            
            List<string> line = new List<string>();
            line.Add(firstString);
            
            word = _words.Find(w => w.Name == firstString);
            int count = word.Sylables;
            int remainingSylables;
            int elements = 0;
            Random rng = new Random();
            do
            {
                if(count < sylablyesAllowed)
                {
                    remainingSylables = sylablyesAllowed - count;
                    List<Word> posiblewords = new List<Word>();

                    word = Words.Find(w => w.Name == line[elements]);
                    Word wrd;
                    foreach (KeyValuePair<string,int> kvp in word.SubsequentWords)
                    {
                         wrd = Words.Find(w => w.Name == kvp.Key);

                        if (wrd.Sylables <= remainingSylables)
                            posiblewords.Add(word);
                    }

                    if (posiblewords.Count == 0)
                        count =+100;
                    else
                    {
                        word = posiblewords[rng.Next(posiblewords.Count() - 1)];
                        line.Add(word.Name);
                        count = +word.Sylables;
                        elements++;
                    }
                }
                else
                {
                    word = Words.Find(w => w.Name == firstString);
                    count =-word.Sylables;
                    count =-100;
                    line.RemoveAt(elements);
                }                
            } while (count != sylablyesAllowed);
            return null;
        }
        /// <summary>
        /// updates the syllables of a word within the network
        /// </summary>
        /// <param name="wordAndSyllables"></param>
        public void SetSylables(Dictionary<string, int> wordAndSyllables)
        {
            foreach(Word w in _words)
            {
                if (wordAndSyllables.ContainsKey(w.Name))
                    w.Sylables = wordAndSyllables[w.Name];
            }
        }

    }
}
