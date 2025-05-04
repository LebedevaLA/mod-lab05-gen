using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Text;
using System.Text.Unicode;
using static System.Runtime.InteropServices.JavaScript.JSType;
using LiveCharts;
using LiveCharts.Wpf;

namespace generator
{
    public class GeneratorBi
    {
        private Dictionary<string, int> bigrams = new Dictionary<string, int>();

        private Dictionary<char, List<(char nextChar, int frequency)>> transitions = new Dictionary<char, List<(char, int)>>();

        private Random random = new Random();
        public GeneratorBi()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "Bi.txt");
            if (!File.Exists(filePath))
            {
                filePath = @"C:\Users\armok\Documents\lebedeva\IASR\mod-lab05-gen\Bi.txt";
            }
            LoadBigrams(filePath);
            BuildTable();
        }
        public GeneratorBi(Dictionary<string, int> bigrams)
        {
            this.bigrams = bigrams;
            BuildTable();
        }
        private void LoadBigrams(string filePath)
        {

            foreach (var line in File.ReadLines(filePath, Encoding.UTF8))
            {
                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 3) continue;

                string bigram = parts[1].Trim().ToLower();
                if (bigram.Length != 2) continue;

                if (int.TryParse(parts[2], out int frequency))
                {
                    bigrams[bigram] = frequency;
                    //Console.WriteLine(bigram);
                }
            }
        }
        private void BuildTable()
        {
            foreach (var pair in bigrams)
            {
                char firstChar = pair.Key[0];
                char nextChar = pair.Key[1];
                int freq = pair.Value;

                if (!transitions.ContainsKey(firstChar))
                {
                    transitions[firstChar] = new List<(char, int)>();
                }
                transitions[firstChar].Add((nextChar, freq));
            }
        }
        public char GetNextChar(char currentChar)
        {
            if (!transitions.ContainsKey(currentChar)) return '\n';
            var candidates = transitions[currentChar];
            
            int totalFrequency = candidates.Sum(c => c.frequency);
            int randomValue = random.Next(totalFrequency);

            int curr = 0;
            foreach (var (nextChar, freq) in candidates)
            {
                curr += freq;
                if (randomValue < curr)
                    return nextChar;
            }

            return candidates.Last().nextChar;
        }
        public string GenerateText(int len)
        {
            
            var result = "";

            char currentChar = transitions.Keys.ElementAt(random.Next(transitions.Count));
            result += currentChar;

            for (int i = 1; i < len; i++)
            {
                currentChar = GetNextChar(currentChar);
                if (currentChar == '\n') break;
                result += currentChar;
            }

            return result;
        }
    };
    public class GeneratorWords
    {
        private Dictionary<string, int> words = new Dictionary<string, int>();

        private Random random = new Random();

        private int sumw = 0;
        public GeneratorWords()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "Bi.txt");
            if (!File.Exists(filePath))
            {
                filePath = @"C:\Users\armok\Documents\lebedeva\IASR\mod-lab05-gen\Words.txt";
            }
            LoadWords(filePath);
            
        }
        public GeneratorWords(Dictionary<string, int> words)
        {
            this.words = words;
        }
        private void LoadWords(string filePath)
        {

            foreach (var line in File.ReadLines(filePath, Encoding.UTF8))
            {
                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;

                string word = parts[1].Trim().ToLower();
                float freqf;
                if (float.TryParse(parts[4], NumberStyles.Float, CultureInfo.InvariantCulture, out freqf))
                {
                    int freq = (int)Math.Round(freqf, 0);
                    words[word] = freq;
                    sumw += words[word];
                }
                
                
            }
        }

   

        public string getNextWord()
        {
            int randomNumber = random.Next(0, sumw);
            int curr = 0;

            foreach (var word in words)
            {
                curr += word.Value;

                if (randomNumber < curr)
                {
                    return word.Key; 
                }
            }
            return "";
        }
        
        public string GenerateSentences(int len)
        {
            string text = "";
            string first = words.Keys.ElementAt(random.Next(words.Count));
            text += first;
            text += ' ';
            for (int i = 0; i < len; i++)
            {
                string next = getNextWord();
                if (next != "")
                {
                    text += next;
                    text += ' ';
                }
            }

            return text;
        }
        
        
    };
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new GeneratorBi();

            string text = generator.GenerateText(1000);

            File.WriteAllText("C:/Users/armok/Documents/lebedeva/IASR/mod-lab05-gen/Results/gen1.txt", text);
            Console.WriteLine("Текст успешно сгенерирован и сохранён");

            var generator1 = new GeneratorWords();

            string text1 = generator1.GenerateSentences(1000);

            File.WriteAllText("C:/Users/armok/Documents/lebedeva/IASR/mod-lab05-gen/Results/gen2.txt", text1);
            Console.WriteLine("Текст успешно сгенерирован и сохранён");

        }
    }
}

