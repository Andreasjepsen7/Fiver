﻿using System;
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
        List<string> words = ReadWordsFromFile(inputFilePath);

        if (words.Count < 3)
        {
            Console.WriteLine("There are not enough words in the file to form combinations.");
            return;
        }

        Console.WriteLine("Generating and printing possible combinations with real words...");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Generate all possible combinations of 5 words
        var wordCombinations = GenerateWordCombinations(words, 5);

        foreach (var combination in wordCombinations)
        {
            PrintWordsPerLine(combination, 5);
            Console.WriteLine();
        }

        stopwatch.Stop();
        Console.WriteLine($"Total combinations generated: {combinationCount}");
        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
    }

    static List<string> ReadWordsFromFile(string filePath)
    {
        List<string> words = new List<string>();

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
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

    static List<List<string>> GenerateWordCombinations(List<string> words, int combinationLength)
    {
        List<List<string>> combinations = new List<List<string>>();
        GenerateWordCombinationsHelper(words, combinationLength, new List<string>(), combinations, 0);
        return combinations;
    }

    static void GenerateWordCombinationsHelper(List<string> words, int combinationLength, List<string> currentCombination, List<List<string>> combinations, int startIndex)
    {
        if (currentCombination.Count == combinationLength)
        {
            combinations.Add(new List<string>(currentCombination));
            combinationCount++;
            return;
        }

        for (int i = startIndex; i < words.Count; i++)
        {
            string word = words[i];
            if (!currentCombination.Any(w => word.Any(c => w.Contains(c))))
            {
                currentCombination.Add(word);
                GenerateWordCombinationsHelper(words, combinationLength, currentCombination, combinations, i + 1);
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
