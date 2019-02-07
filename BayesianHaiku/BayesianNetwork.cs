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
                                if (i != corpus.Count() - 1 && !String.IsNullOrEmpty(corpus[i]))
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
                            Name = corpus[i],
                            Syllables = SyllableCount(corpus[i])
                        };

                        if (i != corpus.Count() - 1 && corpus.Count() != 0 && !String.IsNullOrEmpty(corpus[i]))
                            newWord.SubsequentWords.Add(corpus[i + 1], 1);
                        _words.Add(newWord);

                    }
                }
            }
        }
        //https://codereview.stackexchange.com/questions/9972/syllable-counting-function stolen fuction
        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private int SyllableCount(string word)
        {
            word = word.ToLower().Trim();
            bool lastWasVowel =false;
            var vowels = new[] { 'a', 'e', 'i', 'o', 'u', 'y' };
            int count = 0;

            //a string is an IEnumerable<char>; convenient.
            foreach (var c in word)
            {
                if (vowels.Contains(c))
                {
                    if (!lastWasVowel)
                        count++;
                    lastWasVowel = true;
                }
                else
                    lastWasVowel = false;
            }

            if ((word.EndsWith("e") || (word.EndsWith("es") || word.EndsWith("ed")))
                  && !word.EndsWith("le"))
                count--;

            if (count == 0)
                count = 1;

            return count;
        }
    
        public List<string[]> HaikuCreator()
        {
            Random rng = new Random();
            string start = _words[rng.Next(_words.Count() - 1)].Name;
            List<string[]> haiku = new List<string[]>();

            string[] lineOne = BuildLine(5, start, true);
            

            haiku.Add(lineOne);
            string[] lineTwo = BuildLine(7, lineOne[lineOne.Count()-1], false);
            haiku.Add(lineTwo);
            string[] lineThree = BuildLine(5, lineTwo[lineTwo.Count() - 1], false); 
            haiku.Add(lineThree);

            return haiku;
        }
        private string[] BuildLine(int sylablyesAllowed, string firstString, bool firstLine)
        {
            Word word;
            int count = 0;
            int remainingSylables;
            Random rng = new Random();
            List<string> line = new List<string>();
            
            word = _words.Find(w => w.Name == firstString);

            if (firstLine)
            {
                line.Add(firstString);
                count = word.Syllables;
            }

            do
            {
                if(count < sylablyesAllowed)
                {
                    remainingSylables = sylablyesAllowed - count;
                    List<Word> posiblewords = new List<Word>();
                    if(firstLine && line.Count() >0)
                        word = Words.Find(w => w.Name == line[line.Count()-1]);
                    Word wrd;

                    foreach (KeyValuePair<string, int> kvp in word.SubsequentWords.OrderByDescending(pair => pair.Value).Take(word.SubsequentWords.Count()/2))
                    {
                        if (!string.IsNullOrWhiteSpace(kvp.Key))
                        {
                            wrd = Words.Find(w => w.Name == kvp.Key);

                            if (wrd.Syllables <= remainingSylables && !string.IsNullOrWhiteSpace(wrd.Name))
                                posiblewords.Add(wrd);
                        }
                    }

                    if (posiblewords.Count == 0)
                        count +=100;
                    else
                    {
                        word = posiblewords[rng.Next(posiblewords.Count() - 1)];
                        line.Add(word.Name);
                        count += word.Syllables;
                    }
                }
                else
                {
                    if (line.Count > 0)
                    {
                        word = Words.Find(w => w.Name == firstString);
                        count -= word.Syllables;
                        count -= 100;
                        line.RemoveAt(line.Count() - 1);
                    }
                    else
                    {
                        word = _words[rng.Next(_words.Count() - 1)];
                        count -= 100;
                    }

                }                
            } while (count != sylablyesAllowed);
            return line.ToArray();
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
                    w.Syllables = wordAndSyllables[w.Name];
            }
        }

    }
}
