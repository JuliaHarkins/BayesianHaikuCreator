using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianHaiku
{
    class Bayesian
    {
        /// <summary>
        /// The total amount of words used within the AI
        /// </summary>
        private int _totalWords;
        /// <summary>
        /// A list of the unique words used in the AI
        /// </summary>
        private List<Word> _words;
        /// <summary>
        /// The name of the file in which the AI is contained
        /// </summary>
        private string _fileName;

        /// <summary>
        /// The unique words in the AI
        /// </summary>
        public List<Word> Words { get { return _words; } set { _words = value; } }
        /// <summary>
        /// The AI's file Name
        /// </summary>
        public string FileName { get { return _fileName; } set { _fileName = value; } }
        /// <summary>
        /// The total Words within the AI
        /// </summary>
        public int TotalWords { get { return _totalWords; } set { _totalWords = value; } }

        /// <summary>
        /// The constructor for the AI
        /// </summary>
        public Bayesian()
        {
            _totalWords = 0;
            _words = new List<Word>();
        }
        /// <summary>
        /// Takes a list of words and turns them into something that makes sense to the network
        /// </summary>
        /// <param name="corpus">The words in the training data</param>
        public void Train(string[] corpus)
        {
            bool alreadyExistingWord;

            for (int i = 0; i < corpus.Count(); i++)
            {
                //makes sure the the string is not empty
                if (!String.IsNullOrEmpty(corpus[i]))
                {
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
                                    
                                    if (i > 2)
                                    {
                                        List<string> pastWord = new List<string>();
                                        pastWord.Add(corpus[i - 3]);
                                        pastWord.Add(corpus[i - 2]);
                                        pastWord.Add(corpus[i - 1]);
                                        w.PreviousWords.Add(pastWord.ToArray());
                                    }
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

                        if (i > 2)
                        {
                            List<string> pastWord = new List<string>();
                            pastWord.Add(corpus[i - 3]);
                            pastWord.Add(corpus[i - 2]);
                            pastWord.Add(corpus[i - 1]);
                            newWord.PreviousWords.Add(pastWord.ToArray());
                        }
                    }
                }
            }
            foreach(Word w in this.Words)
            {
                w.Syllables = SyllableCount(w.Name);
            }
        }
        //https://codereview.stackexchange.com/questions/9972/syllable-counting-function stolen fuction
        /// <summary>
        /// Counts the syllables in a word
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private int SyllableCount(string word)
        {
            word = word.ToLower().Trim();
            bool lastWasVowel = false;
            Char[] vowels = new[] { 'a', 'e', 'i', 'o', 'u', 'y' };
            int count = 0;

            //a string is an IEnumerable<char>; convenient.
            foreach (Char c in word)
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


        /// <summary>
        /// Creates a haiku based off the known network knowledge
        /// </summary>
        /// <returns></returns>
        public List<string[]> CreateHaiku()
        {
            Random rng = new Random();
            //picks a random word to start on
            string start = _words[rng.Next(_words.Count() - 1)].Name;

            //creates a list holding each of the lines
            List<string[]> haiku = new List<string[]>();

            //builds each of the three lines
            string[] lineOne = BuildLine(5, start, true);
            haiku.Add(lineOne);
            string[] lineTwo = BuildLine(7, lineOne[lineOne.Count()-1], false);
            haiku.Add(lineTwo);
            string[] lineThree = BuildLine(5, lineTwo[lineTwo.Count() - 1], false); 
            haiku.Add(lineThree);

            return haiku;
        }

        /// <summary>
        /// Builds the indevidual lines within the haiku
        /// </summary>
        private string[] BuildLine(int maxSyllables, string baseString, bool firstLine)
        {
            Word word;
            int count = 0;
            int remainingSylables;
            Random rng = new Random();
            List<string> line = new List<string>();

            word = _words.Find(w => w.Name == baseString);

            //added the first few words
            if (firstLine)
            {                
                //list of diffferent words that could start the haiku
                string[] list = word.PreviousWords[rng.Next(word.PreviousWords.Count() - 1)];
                foreach (string s in list)
                {
                    //checks if the word can be added without going over the syllable limit and if the word is in fact a real word
                    if (count < maxSyllables && !string.IsNullOrEmpty(s) && !int.TryParse(s, out int fail))
                    {
                        line.Add(s);
                        word = Words.Find(w => w.Name == s);
                        count += word.Syllables;
                    }
                }
            }

            //loops to add more words to the haikus line
            do
            {
                //checks if the line is still within the syllable limit
                if (count < maxSyllables)
                {
                    //updates the tracker fro how many syllables are required
                    remainingSylables = maxSyllables - count;
                    //a list of potential words that could follow
                    List<Word> posiblewords = new List<Word>();
                    
                    //the word that is currently being worked with
                    Word wrd;
                    
                    foreach (KeyValuePair<string, int> kvp in word.SubsequentWords.OrderByDescending(pair => pair.Value).Take(word.SubsequentWords.Count()))
                    {
                        //make sure that something is being added
                        if (!string.IsNullOrWhiteSpace(kvp.Key)) {

                            //finding the word based off the name of it
                                wrd = Words.Find(w => w.Name == kvp.Key);

                            //makes sure the word could be used
                            if (wrd.Syllables <= remainingSylables && !string.IsNullOrWhiteSpace(wrd.Name))
                            {
                                if (line.Count() > 2)
                                {
                                    //looks for places where the word has been used following the 3 previous words
                                    List<string> previousHaikuWords = new List<string>();
                                    previousHaikuWords.Add(line[line.Count() - 3]);
                                    previousHaikuWords.Add(line[line.Count() - 2]);
                                    previousHaikuWords.Add(line[line.Count() - 1]);

                                    if (wrd.PreviousWords.Contains(previousHaikuWords.ToArray()))
                                        posiblewords.Add(wrd);
                                }
                                else
                                    posiblewords.Add(wrd);
                            }
                        }                       
                    }

                    if (posiblewords.Count == 0)
                        count += 100;
                    else
                    {
                        if (line.Count() > 2)
                        {

                            Dictionary<string, float> probibilityThisWordShouldFollow = new Dictionary<string, float>();
                            Word previousWord = Words.Find(w => w.Name == line[line.Count() - 1]);
                            int probibilityOfPreviousWord = previousWord.AppearanceCount;

                            foreach (Word w in posiblewords)
                            {
                                float probibilityOfBothWordsOccurring = previousWord.SubsequentWords[w.Name];
                                float probibiliyOfPreviousWordOccurringWithAnotherWord = previousWord.AppearanceCount - probibilityOfBothWordsOccurring;
                                float probibiliyOfPreviousWordOccurring = _totalWords / previousWord.AppearanceCount;
                                float probilityOfPreviousWordNotOccurring = _totalWords - probibiliyOfPreviousWordOccurring;

                                float top = probibilityOfBothWordsOccurring * probibiliyOfPreviousWordOccurring;
                                float bot = top + probibiliyOfPreviousWordOccurringWithAnotherWord * probilityOfPreviousWordNotOccurring;

                                float probiliblyThisWordShouldOccur = top / bot;

                                probibilityThisWordShouldFollow.Add(w.Name, probiliblyThisWordShouldOccur);
                            }
                            //ordered dictionary
                            Dictionary<string, float> orderedDictionary = probibilityThisWordShouldFollow.OrderBy(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
                            int rand = ((orderedDictionary.Count() / 4) - 1);
                            if (rand < 0)
                                rand = 0;
                            word = Words.Find(w => w.Name == orderedDictionary.Keys.ElementAt(rand));
                        }
                        else
                        {
                            int rand = rng.Next(posiblewords.Count() - 1);
                            if (rand < 0)
                                rand = 0;
                            word = posiblewords[rand];
                        }

                        line.Add(word.Name);
                        count += word.Syllables;
                    }
                }
                else if(count>maxSyllables)
                {
                    if (line.Count > 0)
                    {
                        word = Words.Find(w => w.Name == line[line.Count()-1]);
                        count -= word.Syllables;
                        count -= 100;
                        line.RemoveAt(line.Count() - 1);
                    }
                    else
                    {
                        word = _words[rng.Next(_words.Count() - 1)];
                        count = word.Syllables;
                    }

                }
            } while (count != maxSyllables);
            return line.ToArray();
        }
    }
}
