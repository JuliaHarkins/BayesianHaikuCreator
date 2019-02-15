using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianHaiku
{
    class Word
    {
        /// <summary>
        /// the name of the word
        /// </summary>
        private string _name;
        /// <summary>
        /// the amount of syllables within the word
        /// </summary>
        private int _syllables;
        /// <summary>
        /// how often the word appears
        /// </summary>
        private int _apperanceCount;
        /// <summary>
        /// all the words that follow and their frequancy
        /// </summary>
        private Dictionary<string, int> _subsequentWords;
        /// <summary>
        /// The words that came before
        /// </summary>
        private List<string[]> _previousWords;

        /// <summary>
        /// The name of the word
        /// </summary>
        public string Name { get{ return _name; } set { _name = value; } }
        /// <summary>
        /// The amount of sylables within the word
        /// </summary>
        public int Syllables { get { return _syllables; } set { _syllables = value; } }
        /// <summary>
        /// the amount of times this word appeared in the training data
        /// </summary>
        public int AppearanceCount { get { return _apperanceCount; } set { _apperanceCount = value; } }
        /// <summary>
        /// The words that came before
        /// </summary>
        public List<string[]> PreviousWords { get { return _previousWords; } set { _previousWords = value; } }
        /// <summary>
        /// A list of words that normally appear after this word
        /// </summary>
        public Dictionary<string, int> SubsequentWords { get { return _subsequentWords; } set { _subsequentWords = value; } }

        /// <summary>
        /// Constuctor of a word
        /// </summary>
        public Word()
        {
            _apperanceCount = 1;
            _previousWords = new List<string[]>();
            _subsequentWords = new Dictionary<string, int>();
        }
    }
}
