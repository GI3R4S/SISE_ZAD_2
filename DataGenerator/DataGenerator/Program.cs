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
            using (StreamWriter streamWriter = new StreamWriter("trainingSet"))
            {
                Random random = new Random();
                SortedSet<int> numbers = new SortedSet<int>();
                do
                {
                    numbers.Add(random.Next(1, 100));
                } while (numbers.Count != 25);

                foreach(int value in numbers)
                {
                    streamWriter.WriteLine(value.ToString() + " " + Math.Sqrt(value).ToString());
                }
            }
        }
    }
}