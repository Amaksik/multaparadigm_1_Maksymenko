using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace multiParadigmP_1_2
{
    class Program
    {
        ///<summary>Declaration</summary>
        ///some variables to use
        public struct Item
        {
            public Item(int frequency, string chars)
            {
                amount = frequency;
                word = chars;
                pages = new int[10000];
                amountOfPages = 0;

            }
            public int amountOfPages;
            public int amount;
            public string word;
            public int[] pages;
        }
        public static string currentWord = "";
        public static string currentSymbol;
        public static int charInFileCounter = 0;

        public static string[] allWords = new string[90000];
        public static int allWordsPointer = 0 ;

        public static Hashtable TF = new Hashtable();
        public static SortedList<string, int> result = new SortedList<string, int>();

        public static string fileName = @"C:\Users\Anton Maksymenko\Desktop\full.txt";
        
        public static string[] stopWords = { "a", "about", "above", "across", "after", "afterwards", "again", "against", "all",
            "almost", "alone", "along", "already", "also", "although", "always", "am", "among", "amongst", "amoungst", "amount", "an",
            "and", "another", "any", "anyhow", "anyone", "anything", "anyway", "anywhere", "are", "around", "as", "at", "back", "be",
            "became", "because", "become", "becomes", "becoming", "been", "before", "beforehand", "behind", "being", "below", "beside",
            "besides", "between", "beyond", "bill", "both", "bottom", "but", "by", "call", "can", "cannot", "cant", "co", "con", "could",
            "couldnt", "cry", "de", "describe", "detail", "do", "done", "down", "due", "during", "each", "eg", "eight", "either", "eleven",
            "else", "elsewhere", "empty", "enough", "etc", "even", "ever", "every", "everyone", "everything", "everywhere", "except", "few",
            "fifteen", "fify", "fill", "find", "fire", "first", "five", "for", "former", "formerly", "forty", "found", "four", "from", "front",
            "full", "further", "get", "give", "go", "had", "has", "hasnt", "have", "he", "hence", "her", "here", "hereafter", "hereby", "herein",
            "hereupon", "hers", "herself", "him", "himself", "his", "how", "however", "hundred", "ie", "if", "in", "inc", "indeed", "interest",
            "into", "is", "it", "its", "itself", "keep", "last", "latter", "latterly", "least", "less", "ltd", "made", "many", "may", "me",
            "meanwhile", "might", "mill", "mine", "more", "moreover", "most", "mostly", "move", "much", "must", "my", "myself", "name", "namely",
            "neither", "never", "nevertheless", "next", "nine", "no", "nobody", "none", "noone", "nor", "not", "nothing", "now", "nowhere", "of",
            "off", "often", "on", "once", "one", "only", "onto", "or", "other", "others", "otherwise", "our", "ours", "ourselves", "out", "over",
            "own", "part", "per", "perhaps", "please", "put", "rather", "re", "same", "see", "seem", "seemed", "seeming", "seems", "serious", "several",
            "she", "should", "show", "side", "since", "sincere", "six", "sixty", "so", "some", "somehow", "someone", "something", "sometime", "sometimes",
            "somewhere", "still", "such", "system", "take", "ten", "than", "that", "the", "their", "them", "themselves", "then", "thence", "there", "thereafter",
            "thereby", "therefore", "therein", "thereupon", "these", "they", "thick", "thin", "third", "this", "those", "though", "three", "through", "throughout",
            "thru", "thus", "to", "together", "too", "top", "toward", "towards", "twelve", "twenty", "two", "un", "under", "until", "up", "upon", "us", "very",
            "via", "was", "we", "well", "were", "what", "whatever", "when", "whence", "whenever", "where", "whereafter", "whereas", "whereby", "wherein",
            "whereupon", "wherever", "whether", "which", "while", "whither", "who", "whoever", "whole", "whom", "whose", "why", "will", "with", "within",
            "without", "would", "yet", "you", "your", "yours", "yourself", "yourselves", "the" };


        static void Main(string[] args)
        {
            //reading file to a string to then operate with it
            string text = File.ReadAllText(fileName);
            //text = "White tigers live mostly in India Wild lions live mostly in Africa";
            goto GetAllWords;


        GetAllWords:
            //while we not check all characters in string continue 
            if (charInFileCounter >= text.Length)
            {
                goto Ending;
            }


            currentSymbol = text[charInFileCounter].ToString();

            //check if our current symbol is an letter
            if (!(currentSymbol == "." || currentSymbol == "," || currentSymbol == "!" || currentSymbol == "?" || currentSymbol == ":" ||
                currentSymbol == ";" || currentSymbol == "(" || currentSymbol == ")" || currentSymbol == "[" || currentSymbol == "]" ||
                currentSymbol == "*" || currentSymbol == "-" || currentSymbol == "\n" || currentSymbol == "\r" || currentSymbol == '"'.ToString() ||
                currentSymbol == "'" || currentSymbol == "&" || currentSymbol == "" || currentSymbol == " " || currentSymbol == "_"))
            {

                int ascii = (int)Convert.ToChar(currentSymbol);
                if (ascii >= 'A' && ascii <= 'Z')
                //transform to lower start
                {
                    // change the character
                    char c = (char)(text[charInFileCounter] + 32);
                    currentWord += c;
                    charInFileCounter++;
                    goto GetAllWords;
                }
                //transform to lower end
                else
                {
                    currentWord += currentSymbol;
                    charInFileCounter++;
                    //repetition to add last word in a file
                    if (charInFileCounter == text.Length)
                    {

                        //check if it is not "stop-word"
                        if (Array.Exists(stopWords, element => element == currentWord))
                        {
                            charInFileCounter++;
                            currentWord = "";
                            goto GetAllWords;
                        }
                        else
                        {
                            if (TF.Contains(currentWord))
                            {
                                var element = TF[currentWord];
                                TF[currentWord] = (object)((int)element + 1);
                                var elementnew = TF[currentWord];

                            }
                            else
                            {
                                TF.Add(currentWord, 1);
                            }

                            charInFileCounter++;
                            currentWord = "";
                            goto GetAllWords;
                        }
                    }
                    goto GetAllWords;
                }

            }
            else if (currentWord == "")
            {
                charInFileCounter++;
                goto GetAllWords;
            }
            //if current symbol is not a letter then add word that we make to array
            else
            {
                //check if it is not "stop-word"
                if (Array.Exists(stopWords, element => element == currentWord))
                {
                    charInFileCounter++;
                    currentWord = "";
                    goto GetAllWords;
                }
                //if everything fine with word
                else
                {
                    allWords[allWordsPointer] = currentWord;
                    allWordsPointer++;

                    //cheching if we already has that word in array
                    if (TF.Contains(currentWord))
                    {
                        //changing the amount of repetitions if true
                        var element = TF[currentWord];
                        TF[currentWord] = (object)((int)element + 1);
                        var elementnew = TF[currentWord];

                    }
                    else
                    {
                        //adding word if it`s enough long
                        if (currentWord.Length >= 3 || currentWord == "i")
                        { TF.Add(currentWord, 1); }
                    }
                    //process next character
                    charInFileCounter++;
                    currentWord = "";
                    goto GetAllWords;
                }

            }

        
        Ending:
            //using enumerator to replace foreach 
            Item[] results = new Item[TF.Count];
            var tfcounter = 0;
            var counter = TF.GetEnumerator();
            counter.Reset();
            counter.MoveNext();

        DuplicatingData:
            //loop to add data to array
            if (tfcounter < TF.Count)
            {
                var key = counter.Key;
                var value = counter.Value;

                results[tfcounter] = new Item((int)value, key.ToString());

                tfcounter++;
                if (counter.MoveNext())
                {
                    goto DuplicatingData;
                }

            }
            //now we have array with all words


            Item temp;
            /// Sorting by strings using bubble sort
            int j = 0;
            int n = results.Length;
        cyclestart:
            if (j < n - 1)
            {
                int i = j + 1;
            innercycle:
                if (i < n)
                {

                    if (results[j].word.CompareTo(results[i].word) > 0)
                    {
                        temp = results[j];
                        results[j] = results[i];
                        results[i] = temp;
                    }
                    i++;
                    goto innercycle;

                }
                j++;
                goto cyclestart;
            }

            goto FindPages;



        FindPages:

            int pointerInWords = 0;
            int pointerInItems = 0;
            int currentPage = 1;

loopThroughItems:
            if(pointerInItems< results.Length)
            {
                var currentItem = results[pointerInItems];
            loopThroughWordsForItem:
                if (pointerInWords > 254* currentPage)
                {
                    currentPage++;
                }
                if (pointerInWords < allWordsPointer)
                {

                    if (allWords[pointerInWords] == currentItem.word)
                    {
                        currentItem.pages[currentItem.amountOfPages] = currentPage;
                        currentItem.amountOfPages++;
                        pointerInWords = (pointerInWords / 254 + 1) * 254;
                        goto loopThroughWordsForItem;
                    }
                    pointerInWords++;
                    goto loopThroughWordsForItem;
                }
                else
                {
                    pointerInWords = 0;
                    pointerInItems++;
                    currentPage = 0;
                    goto loopThroughItems;
                }
            }

            









            int displaycounter = 0;
            int amountOfElements = results.Length;
        loopstart:

            if ( displaycounter < amountOfElements)
            {
                if (results[displaycounter].amount > 100)
                {
                    displaycounter++;
                    goto loopstart;
                }
                Console.Write(results[displaycounter].word + " - ");
                int pagesCounter = 0;
                
     displayEachWord:
                if(pagesCounter< results[displaycounter].amount)
                {
                    if(results[displaycounter].pages[pagesCounter] == 0)
                    {
                        pagesCounter++;
                        goto displayEachWord;
                    }
                    Console.Write(results[displaycounter].pages[pagesCounter]);
                    if(pagesCounter < results[displaycounter].amount - 1)
                    {
                        Console.Write(", ");
                    }
                    pagesCounter++;
                    goto displayEachWord;

                }
                else
                {
                    Console.Write("\n");
                    displaycounter++;
                    goto loopstart;
                }
                
                
            }

        }


    }

}
