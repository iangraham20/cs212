/* 
 * Program 2 - Babble
 * Ian Christensen (igc2)
 * Prof. Harry Plantinga
 * CS-212-A
 * Fall, 2017
 */

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace BabbleSample
{
    public partial class MainWindow : Window
    {
        private string input;                               // input file
        private string[] words = { };                       // input file broken into array of words
        private int wordCount = 200;                        // number of words to babble
        private Dictionary<string, ArrayList>
        hashtable = new Dictionary<string, ArrayList>();    // hashtable used to map keys to their value options
        private int orderSelection = 0;                     // selected order of 
        private string[] hashWords;                         // hashed values in array of words
        private string searchKey;                           // 
        private Random random = new Random();               // random integer generator

        public MainWindow()
        {
            InitializeComponent();
        }
        /* LoadButton_Click function
         * This function recognizes when a user clicks on the load button, takes the loaded text file,
         * and splits the text into an array of words
         * 
         * input:   object          sender  ???
         *          RoutedEventArgs e       ???
         * output:  the array "words" is filled
         */
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "Sample",                   // Default file name
                DefaultExt = ".txt",                   // Default file extension
                Filter = "Text documents (.txt)|*.txt" // Filter files by extension
            };
            // Show open file dialog box
            if ((bool)ofd.ShowDialog())
            {
                textBlock1.Text = "Loading file " + ofd.FileName + "\n";// print out that the file is being loaded
                input = System.IO.File.ReadAllText(ofd.FileName);       // read file
                words = Regex.Split(input, @"\s+");                     // split into array of words
            }
            AnalyzeInput(OrderComboBox.SelectedIndex);
            textBlock1.Text = "Word Count: " + words.Length.ToString() + "\nUnique Keys: " + hashtable.Count.ToString();
        }
        /* AnalyzeInput function
         * This function takes the array of words and creates keys using the unique words (or unique word groups 
         * depending on the order of analysis) in the array and maps their respective following words in an arraylist
         * 
         * input:   int order   This determines the number of words that are chained together in each key
         * output:  dictionary hashtable is filled 
         */
        private void AnalyzeInput(int order)
        {
            hashtable.Clear();  // empty hashtable values from any previous analysis
            string key, val;    // initialize the string key and val variables
            if (order > 0)      // check to see that the order selected is higher than zero
            {
                orderSelection = order;
                for (int i = 0; i < words.Length - (order + 1); i++)    // loops until the last potential key and value is accounted for given the order
                {
                    key = val = "";                         // set the key and val to an empty string in case of previous analysis values
                    for (uint j = 0; j < order; j++)
                    {
                        key += words[i + j];
                    }
                    val = words[i + order];                 // assign val to the number of words specified by the order
                    if (!hashtable.ContainsKey(key))        // check to see if the key exists
                    {
                        hashtable[key] = new ArrayList();   // create a key for the unique word and its arraylist
                        hashtable[key].Add(val);            // add the value following the key to the arraylist
                    }
                    else
                        hashtable[key].Add(val);            // add the value to the arraylist of the previously created key
                }
            }
        }
        /* BabbleButton_Click function
         * This function recognizes when a user clicks on the Babble button, and 
         * 
         * input:   object          sender  ???
         *          RoutedEventArgs e       ???
         * output:  The babbled text
         */
        private void BabbleButton_Click(object sender, RoutedEventArgs e)
        {
            hashWords = new string[Math.Min(wordCount, words.Length)];  // create a new string either the length of the specified word count or the length of the given text file 
            textBlock1.Text += "\n";
            for (int i = 0; i < orderSelection; i++)
            {
                hashWords[i] = words[i];
                textBlock1.Text += " " + hashWords[i];                  // output the hashWords array values with a space as the delimitor 
            }
            for (int i = orderSelection; i < Math.Min(wordCount, words.Length); i++)    // loop through beginning with order and ending with either the length of the specified word count or the length of the given text file
            {
                searchKey = "";                             // set searchKey to an empty string in case of previous values
                for (int j = orderSelection; j > 0; j--)
                {
                    searchKey += hashWords[i - j];          // set searchkey to the given key value
                }
                try // this try block checks for the last word of the text file and sets its arraylist value to the first word in the text
                {
                    hashWords[i] = hashtable[searchKey][random.Next(0, hashtable[searchKey].Count)].ToString(); //
                } catch {
                    searchKey = "";                         // set searchKey to an empty string in case of previous values
                    for (int j = orderSelection; j > 0; j--)
                    {
                        hashWords[i - j] = words[orderSelection - j]; // reassigns arraylist value
                    }
                    hashWords[i] = hashtable[searchKey][random.Next(0, hashtable[searchKey].Count)].ToString();
                }
                textBlock1.Text += " " + hashWords[i];  // output the hashWords array values with a space as the delimitor
            }
        }

        /* OrderComboBox_SelectionChanged function
         * This function recognizes when the order has been changed and displays the output values
         * 
         * input:   object                      sender  ???
         *          SelectionChangedEventArgs   e       ???
         * output:  Word Count      The total number of words in the text file
         *          Unique Words    The total number of unique keys in the hashtable
         */
        private void OrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AnalyzeInput(OrderComboBox.SelectedIndex);
            textBlock1.Text = "Word Count: " + words.Length.ToString() + "\nUnique Keys: " + hashtable.Count.ToString();   // output the word count and the number of unique key
        }
    }
}