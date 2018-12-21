using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BayesianHaiku
{
    class Word
    {
        private string _name;
        private int _sylables;
        private int _apperanceCount;
        private Dictionary<string, int> _subsequentWords;
    
        /// <summary>
        /// The name of the word
        /// </summary>
        public string Name { get{ return _name; } set { _name = value; } }
        /// <summary>
        /// The amount of sylables within the word
        /// </summary>
        public int Sylables { get { return _sylables; } set { _sylables = value; } }
        /// <summary>
        /// the amount of times this word appeared in the training data
        /// </summary>
        public int AppearanceCount { get { return _apperanceCount; } set { _apperanceCount = value; } }
        /// <summary>
        /// A list of words that normally appear befor this word
        /// </summary>
        public Dictionary<string, int> SubsequentWords { get { return _subsequentWords; } set { _subsequentWords = value; } }

        /// <summary>
        /// Constuctor of a word
        /// </summary>
        public Word()
        {

        }
    }
}
