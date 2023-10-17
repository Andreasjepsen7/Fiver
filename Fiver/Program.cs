using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string inputFilePath = "C:\\Users\\Andreas\\source\\repos\\Fiver\\Fiver\\five.txt"; // Update with your text file path
        List<string> words = ReadWordsFromFile(inputFilePath);

        if (words.Count < 5)
        {
            Console.WriteLine("There are not enough words in the text file to form combinations.");
            return;
        }

        Console.WriteLine("Generating and printing possible combinations...");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Generate all possible combinations of letters in each word
        var letterCombinations = GenerateLetterCombinations(words);

        foreach (var combination in letterCombinations)
        {
            PrintWordsPerLine(combination, 5);
        }

        stopwatch.Stop();
        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
    }

    static List<string> ReadWordsFromFile(string filePath)
    {
        List<string> words = new List<string>();

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = sr.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    words.AddRange(line.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while reading the file: " + ex.Message);
        }

        return words;
    }

    static List<List<string>> GenerateLetterCombinations(List<string> words)
    {
        List<List<string>> combinations = new List<List<string>>();

        foreach (var word in words)
        {
            List<string> letterCombinations = GeneratePermutations(word);
            combinations.Add(letterCombinations);
        }

        return combinations;
    }

    static List<string> GeneratePermutations(string word)
    {
        List<string> permutations = new List<string>();
        GeneratePermutationsHelper(word.ToCharArray(), 0, permutations);
        return permutations;
    }

    static void GeneratePermutationsHelper(char[] chars, int index, List<string> permutations)
    {
        if (index == chars.Length - 1)
        {
            permutations.Add(new string(chars));
        }
        else
        {
            for (int i = index; i < chars.Length; i++)
            {
                Swap(chars, index, i);
                GeneratePermutationsHelper(chars, index + 1, permutations);
                Swap(chars, index, i); // Restore the original order
            }
        }
    }

    static void Swap(char[] chars, int i, int j)
    {
        char temp = chars[i];
        chars[i] = chars[j];
        chars[j] = temp;
    }

    static void PrintWordsPerLine(List<string> words, int wordsPerLine)
    {
        StringBuilder lineBuilder = new StringBuilder();

        foreach (var word in words)
        {
            lineBuilder.Append(word).Append(" ");

            if (lineBuilder.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length % wordsPerLine == 0)
            {
                Console.WriteLine(lineBuilder.ToString().Trim());
                lineBuilder.Clear();
            }
        }

        if (lineBuilder.Length > 0)
        {
            Console.WriteLine(lineBuilder.ToString().Trim());
        }
    }
}
