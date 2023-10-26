using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

class Program
{
    private static int combinationCount = 0;
    private static Dictionary<int, string> _dictionary;

    static void Main(string[] args)
    {
        try
        {
            // output og input paths
            string outputFilePath = "C:\\Users\\Andreas\\source\\repos\\Fiver\\Fiver\\combi.txt";
            string inputFilePath = "C:\\Users\\Andreas\\source\\repos\\Fiver\\Fiver\\five.txt";

            // læser input filen
            string[] wordsArray = ReadWordsFromFile(inputFilePath);
            List<string> words = wordsArray.ToList();

            int originalWordCount = words.Count;

            // filter til 5 bogstavs ord
            words = FilterWords(words);

            if (words.Count < 5)
            {
                Console.WriteLine("There are not enough valid 5-letter words in the file to form combinations.");
                return;
            }

            Console.WriteLine($"Valid words count: {words.Count} (out of {originalWordCount})");
            Console.WriteLine("Generating and printing possible combinations with real words...");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            _dictionary = new Dictionary<int, string>();

            // bygger et directory som gemmer ordene som bits
            foreach (string word in words)
            {
                int bitKey = 0;
                foreach (var bit in word)
                {
                    bitKey |= 1 << bit - 'a';
                }
                if (_dictionary.ContainsKey(bitKey)) continue;
                _dictionary.Add(bitKey, word);
            }
            int[] keys = _dictionary.Keys.ToArray();

            // Generere ord kombinationer
            var wordCombinations = GenerateWordCombinations(keys, 5);

            int batchSize = 10;
            int currentIndex = 0;

            using (var file = File.Create(outputFilePath))
            using (var writer = new StreamWriter(file))
            {
                // skriver combinationer i batches
                while (currentIndex < wordCombinations.Count)
                {
                    int endIndex = Math.Min(currentIndex + batchSize, wordCombinations.Count);
                    var batch = wordCombinations.GetRange(currentIndex, endIndex - currentIndex);

                    foreach (var combination in batch)
                    {
                        foreach (var word in combination)
                        {
                            writer.WriteLine(string.Join(" ", word));
                        }
                        writer.WriteLine();
                    }
                    currentIndex = endIndex;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Total combinations generated: {combinationCount}");
            Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    // læser input filen og putter den i et array of strings
    public static string[] ReadWordsFromFile(string filePath)
    {
        List<string> words = new List<string>();
        using (StreamReader reader = new StreamReader(filePath))
        {
            string word;
            while ((word = reader.ReadLine()) != null)
            {
                words.Add(word);
            }
        }
        return words.ToArray();
    }


    // filter
    public static List<string> FilterWords(List<string> words)
    {
        return words
            .Where(word => word.Length == 5 && word.Distinct().Count() == 5)
            .GroupBy(word => string.Concat(word.OrderBy(c => c)))
            .Select(group => group.First()) // Remove anagrams
            .ToList();
    }


    // genererer kombinationer af ord baseret på bits
    static List<List<string>> GenerateWordCombinations(int[] words, int combinationLength)
    {
        List<List<string>> result = new List<List<string>>();
        GenerateWordCombinationsHelper(words, combinationLength, 0, new int[0], result, 0);
        return result;
    }


    // hjælpe method til at genererer ved brug af rekursiv
    static void GenerateWordCombinationsHelper(int[] words, int combinationLength, int key, int[] currentCombination, List<List<string>> result, int startIndex)
    {
        if (currentCombination.Length == combinationLength)
        {
            var list = new List<string>();

            foreach (var bit in currentCombination)
            {
                list.Add(_dictionary[bit]);
            }
            result.Add(list);
            combinationCount++;
            return;
        }

        for (int i = startIndex; i < words.Length; i++)
        {
            int word = words[i];
            if ((key & word) == 0)
            {
                int[] newCombinations = new int[currentCombination.Length + 1];
                currentCombination.CopyTo(newCombinations, 0);
                newCombinations[newCombinations.Length - 1] = word;
                GenerateWordCombinationsHelper(words, combinationLength, key | word, newCombinations, result, i + 1);
            }
        }
    }

    // udskriver specificerede ord per linje
    static void PrintWordsPerLine(List<string> words, int wordsPerLine)
    {
        int count = 0;

        foreach (var word in words)
        {
            Console.Write(word + " ");
            count++;

            if (count % wordsPerLine == 0)
            {
                Console.WriteLine();
            }
        }

        if (count % wordsPerLine != 0)
        {
            Console.WriteLine();
        }
    }
}