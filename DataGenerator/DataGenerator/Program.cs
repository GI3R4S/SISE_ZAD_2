using System;
using System.Collections.Generic;
using System.IO;

namespace DataGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GenerateTrainingData();
        }

        public static void GenerateTrainingData()
        {
            using (StreamWriter streamWriter = new StreamWriter("trainingData10^9"))
            {
                Random random = new Random();
                SortedSet<int> numbers = new SortedSet<int>();
                do
                {
                    numbers.Add(random.Next(100000000, 1000000000));
                } while (numbers.Count !=100);

                foreach(int value in numbers)
                {
                    streamWriter.WriteLine(value.ToString() + " " + Math.Sqrt(value).ToString());
                }
            }
        }
    }
}