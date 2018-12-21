using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MLP
{
    class NeuralNetwork
    {
        static List<KeyValuePair<int, double>> trainingData;
        static List<List<KeyValuePair<int, double>>> testingData;
        static List<double> mins = new List<double>(){ int.MaxValue, int.MaxValue, int.MaxValue};
        public static int hiddenLayerCount = 4;
        public static int NumberOfEpochs = 100000;

        NeuralNetwork()
        {
            trainingData = new List<KeyValuePair<int, double>>();
            testingData = new List<List<KeyValuePair<int, double>>>();
        }
        static void Main(string[] args)
        {
            NeuralNetwork program = new NeuralNetwork();
            LoadTrainingData();
            LoadTestingData();
            Random random = new Random();


            Neuron[] hiddenLayer = new Neuron[hiddenLayerCount];
            Neuron[] outputLayer = new Neuron[1];
            for (int i = 0; i < hiddenLayer.Length; i++)
            {
                hiddenLayer[i] = new Neuron(1, 1);
                hiddenLayer[i].RandomizeValues();
            }
            outputLayer[0] = new Neuron(hiddenLayerCount, 2);
            outputLayer[0].RandomizeValues();

            StringBuilder stringBuilder = new StringBuilder();
            #region TrainingPhase
            for (int i = 0; i < NumberOfEpochs; i++)
            {

                List<int> indexes = GetNumbers(trainingData.Count);
                for (int j = 0; j < trainingData.Count; j++)
                {
                    
                    int index = random.Next(0, indexes.Count);
                    indexes.RemoveAt(index);


                    double[] hiddenLayerInputs = new double[] { trainingData[index].Key };
                    double[] outputLayerInputs = new double[hiddenLayerCount];


                    foreach (Neuron n in hiddenLayer)
                    {
                        n.Inputs = hiddenLayerInputs;
                    }
                    for (int k = 0; k < hiddenLayer.Length; k++)
                    {
                        outputLayerInputs[k] = hiddenLayer[k].Output();
                    }
                    outputLayer[0].Inputs = outputLayerInputs;


                    double diffrence = 0;
                    diffrence = trainingData[index].Value - outputLayer[0].Output();

                    outputLayer[0].Error = diffrence;
                    for (int k = 0; k < hiddenLayer.Length; k++)
                    {
                        hiddenLayer[k].Error = Sigm.FunctionDerivative(hiddenLayer[k].Output()) * outputLayer[0].Error * outputLayer[0].Weights[k];
                        hiddenLayer[k].UpdateWeights();
                    }
                    outputLayer[0].UpdateWeights();
                }

                int iterator = 0;
                foreach(var testData in testingData)
                {
                    double TestingMSE = 0;
                    for(int j = 0; j < testData.Count; j++)
                    {
                        double[] hiddenLayerInputs = new double[] { testData[j].Key };
                        double[] outputLayerInputs = new double[hiddenLayerCount];


                        foreach (Neuron n in hiddenLayer)
                        {
                            n.Inputs = hiddenLayerInputs;
                        }
                        for (int k = 0; k < hiddenLayer.Length; k++)
                        {
                            outputLayerInputs[k] = hiddenLayer[k].Output();
                        }
                        outputLayer[0].Inputs = outputLayerInputs;

                        TestingMSE += Math.Pow(testData[j].Value - outputLayer[0].Output(), 2);
                    }
                    if (TestingMSE / testData.Count < mins[iterator])
                        mins[iterator] = TestingMSE / testData.Count;

                    if (i < 1000)
                    {
                        stringBuilder.Append((TestingMSE / testData.Count).ToString());
                        stringBuilder.Append("\t");
                    }
                    iterator++;
                }
                stringBuilder.Append("\n");
            }

            string errorsFName = hiddenLayer.Length + "n_" + Neuron.LearningFactor + "lf_" + Neuron.MomentumFactor + "mf_errors";
            string minFName = hiddenLayer.Length + "n_" + Neuron.LearningFactor + "lf_" + Neuron.MomentumFactor + "mf_mins";
            using (StreamWriter streamWriter = new StreamWriter(errorsFName))
            {
                streamWriter.Write(stringBuilder.ToString());
            }
            using (StreamWriter streamWriter = new StreamWriter(minFName))
            {
                for(int j = 0; j < mins.Count; j++)
                {
                    streamWriter.Write(mins[j] + "\t");
                }
            }
            #endregion
        }

        public static List<int> GetNumbers(int range)
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < range; i++)
            {
                numbers.Add(i);
            }
            return numbers;
        }

        #region DataExtraction
        public static void LoadTrainingData()
        {
            StreamReader streamReader = new StreamReader("../../trainingSet");
            string[] fileStrings = streamReader.ReadToEnd().Replace('\n', ' ').Split(' ');
            int value = 0;
            double sqrt = 0;

            for(int i = 0; i < fileStrings.Length - 1; i++)
            {
                if(i % 2 == 1)
                {
                    sqrt = Convert.ToDouble(fileStrings[i]);
                    trainingData.Add(new KeyValuePair<int, double>(value, sqrt));
                }
                else
                {
                    value = Convert.ToInt32(fileStrings[i]);
                }
            }
        }


        public static void LoadTestingData()
        {
            string[] fileNames = Directory.GetFiles("../..").Where(p => p.Contains("testingSet")).ToArray();
            foreach(string fileName in fileNames)
            {
                testingData.Add(new List<KeyValuePair<int, double>>());
                int currentIndex = testingData.Count - 1;

                StreamReader streamReader = new StreamReader(fileName);
                string[] fileStrings = streamReader.ReadToEnd().Replace('\n', ' ').Split(' ');
                int value = 0;
                double sqrt = 0;

                for (int i = 0; i < fileStrings.Length - 1; i++)
                {
                    if (i % 2 == 1)
                    {
                        sqrt = Convert.ToDouble(fileStrings[i]);
                        testingData[currentIndex].Add(new KeyValuePair<int, double>(value, sqrt));
                    }
                    else
                    {
                        value = Convert.ToInt32(fileStrings[i]);
                    }
                }
            }
        }
        #endregion
    }
}
