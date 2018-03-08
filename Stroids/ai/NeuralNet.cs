namespace Stroids.ai
{
    public class NeuralNet
    {
        private readonly Matrix _inputLayer;//matrix containing weights between the input nodes and the hidden nodes
        private readonly Matrix _hiddenLayer;//matrix containing weights between the hidden nodes and the second layer hidden nodes
        private readonly Matrix _outputLayer;//matrix containing weights between the second hidden layer nodes and the output nodes

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="inputNodeCount">Input node count</param>
        /// <param name="hiddenNodeCount">Hidden node count</param>
        /// <param name="outputNodeCount">Output node count</param>
        public NeuralNet(int inputNodeCount, int hiddenNodeCount, int outputNodeCount)
        {
            //create first layer weights 
            _inputLayer = Matrix.Random(hiddenNodeCount, inputNodeCount + 1);

            //create second layer weights
            _hiddenLayer = Matrix.Random(hiddenNodeCount, hiddenNodeCount + 1);

            //create second layer weights
            _outputLayer = Matrix.Random(outputNodeCount, hiddenNodeCount + 1);
        }

        /// <summary>
        /// Copy construction
        /// </summary>
        /// <param name="inputLayer"></param>
        /// <param name="hiddenLayer"></param>
        /// <param name="outputLayer"></param>
        private NeuralNet(Matrix inputLayer, Matrix hiddenLayer, Matrix outputLayer)
        {
            this._inputLayer = inputLayer;
            this._hiddenLayer = hiddenLayer;
            this._outputLayer = outputLayer;
        }
        
        /// <summary>
        /// calculate the output values by feeding forward through the neural network
        /// </summary>
        /// <param name="inputsArr"></param>
        /// <returns></returns>
        public float[] Output(float[] inputsArr)
        {
            //add bias 
            var inputsBias = new Matrix(inputsArr).AddBias();
            
            //-----------------------calculate the guessed output

            //apply layer one weights to the inputNodeCount
            var hiddenInputs = _inputLayer.Dot(inputsBias);

            //pass through activation function(sigmoid)
            var hiddenOutputs = hiddenInputs.Activate();

            //add bias
            var hiddenOutputsBias = hiddenOutputs.AddBias();

            //apply layer two weights
            var hiddenInputs2 = _hiddenLayer.Dot(hiddenOutputsBias);
            var hiddenOutputs2 = hiddenInputs2.Activate();
            var hiddenOutputsBias2 = hiddenOutputs2.AddBias();

            //apply level three weights
            var outputInputs = _outputLayer.Dot(hiddenOutputsBias2);
            
            //pass through activation function(sigmoid)
            var outputs = outputInputs.Activate();

            //convert to an array and return
            return outputs.ToArray();
        }

        /// <summary>
        /// mutation function for genetic algorithm
        /// </summary>
        /// <param name="mr">Rate of mutation</param>
        public NeuralNet Mutate(float mr)
        {
            return new NeuralNet(
                _inputLayer.Mutate(mr),
                _hiddenLayer.Mutate(mr),
                _outputLayer.Mutate(mr));
        }

        /// <summary>
        /// crossover function for genetic algorithm
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public NeuralNet Crossover(NeuralNet partner)
        {
            //creates a new child with layer matricies from both parents
            return new NeuralNet(
                _inputLayer.Crossover(partner._inputLayer),
                _hiddenLayer.Crossover(partner._hiddenLayer),
                _outputLayer.Crossover(partner._outputLayer));
        }

        /// <summary>
        /// return a neural net which is a clone of this Neural net
        /// </summary>
        /// <returns></returns>
        public NeuralNet Clone()
        {
            return new NeuralNet(
                _inputLayer.Clone(),
                _hiddenLayer.Clone(),
                _outputLayer.Clone());
        }
    }
}
