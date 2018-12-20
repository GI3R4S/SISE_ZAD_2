using System;
namespace MLP
{
    #region Neuron
    public class Neuron
    {
        public static float LearningFactor = 0.001f;
        public static float MomentumFactor = 0.001f;

        public static bool IsBiasUsed = true;
        #region Definitions
        public double[] Inputs { get; set; }
        public double[] Weights { get; set; }
        public double[] PreviousChanges { get; set; }
        public double PreviousBiasChange { get; set; }
        public double Bias { get; set; }
        public static Random gen = new Random();
        public double Error;
        public int ActivationFunction;
        #endregion
        public Neuron(int inputCount, int newActivationFuntion)
        {
            Weights = new double[inputCount];
            PreviousChanges = new double[inputCount];
            ActivationFunction = newActivationFuntion;
        }

        public double Output()
        {
            double result = 0;
            for (int i = 0; i < Weights.Length; i++)
            {
                result += Weights[i] * Inputs[i];
            }
            if (IsBiasUsed)
            {
                result += Bias;
            }
            if (ActivationFunction == 1)
            {
                result = Sigm.Function(result);
            }
            if (ActivationFunction == 2)
            {
                result = Linear.Function(result);
            }
            return result;
        }
        public void RandomizeValues()
        {
            Bias = (gen.NextDouble() - 0.5) * 2;
            for (int i = 0; i < Weights.Length; i++)
            {
                PreviousChanges[i] = 0;
                PreviousBiasChange = 0;
                Weights[i] = (gen.NextDouble() - 0.5) * 2;
            }
        }

        public void UpdateWeights()
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] += Error * Inputs[i] * LearningFactor + MomentumFactor * PreviousChanges[i];
                PreviousChanges[i] = Error * Inputs[i] * LearningFactor + MomentumFactor * PreviousChanges[i];
            }

            if (IsBiasUsed)
            {
                Bias += Error * LearningFactor + MomentumFactor * PreviousBiasChange;
                PreviousBiasChange = Error * LearningFactor + MomentumFactor * PreviousBiasChange;
            }
        }
        #endregion

    }
}