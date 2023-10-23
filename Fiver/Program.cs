using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

class Program
{
    private static int combinationCount = 0;

    static void Main(string[] args)
    {
        string inputFilePath = @"C:\Users\Andreas\source\repos\Fiver\Fiver\five.txt";
        string[] wordsArray = ReadWordsFromFile(inputFilePath);
        List<string> words = wordsArray.ToList();

        int originalWordCount = words.Count; // Store the original word count

        // Remove anagrams and words with lengths above or below 5
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

        // Generate all possible combinations of 5 words with 5 unique letters each
        var wordCombinations = GenerateWordCombinations(words, 5);

        int batchSize = 10;
        int currentIndex = 0;

        while (currentIndex < wordCombinations.Count)
        {
            int endIndex = Math.Min(currentIndex + batchSize, wordCombinations.Count);
            var batch = wordCombinations.GetRange(currentIndex, endIndex - currentIndex);

            foreach (var combination in batch)
            {
                PrintWordsPerLine(combination, 5);
                Console.WriteLine();
            }

            currentIndex = endIndex;

            // Check if the user wants to see more combinations
            if (currentIndex < wordCombinations.Count)
            {
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"Total combinations generated: {combinationCount}");
        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
    }

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

    public static List<string> FilterWords(List<string> words)
    {
        // Remove anagrams and words with lengths above or below 5
        return words
            .Where(word => word.Length == 5 && word.Distinct().Count() == 5)
            .GroupBy(word => string.Concat(word.OrderBy(c => c)))
            .Select(group => group.First()) // Remove anagrams
            .ToList();
    }

    static List<List<string>> GenerateWordCombinations(List<string> words, int combinationLength)
    {
        List<List<string>> result = new List<List<string>>();
        GenerateWordCombinationsHelper(words, combinationLength, new List<string>(), result, 0);
        return result;
    }

    static void GenerateWordCombinationsHelper(List<string> words, int combinationLength, List<string> currentCombination, List<List<string>> result, int startIndex)
    {
        if (currentCombination.Count == combinationLength)
        {
            result.Add(new List<string>(currentCombination));
            combinationCount++;
            return;
        }

        for (int i = startIndex; i < words.Count; i++)
        {
            string word = words[i];
            if (!currentCombination.Any(w => word.Any(c => w.Contains(c))))
            {
                currentCombination.Add(word);
                GenerateWordCombinationsHelper(words, combinationLength, currentCombination, result, i + 1);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }
    }

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
