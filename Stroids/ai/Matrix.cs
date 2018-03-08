using System;
using System.Diagnostics.Contracts;

namespace Stroids.ai
{
    /// <summary>
    /// A matrix value with variable row and column size
    /// </summary>
    public struct Matrix
    {
        private readonly int _rows;
        private readonly int _cols;
        private readonly float[,] _matrix;

        /// <summary>
        /// Creates a table matrix from the parameter array
        /// </summary>
        private Matrix(float[,] f)
        {
            _rows = f.GetLength(0);
            _cols = f.GetLength(1);
            _matrix = f;
        }

        /// <summary>
        /// Creates a single column matrix from the parameter array
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public Matrix (float[] arr)
        {
            _rows = arr.Length;
            _cols = 1;

            var f = new float[arr.Length, 1];
            for (int i = 0; i < arr.Length; i++)
            {
                f[i, 0] = arr[i];
            }
            _matrix = f;
        }

        /// <summary>
        /// return a matrix which is this matrix dot product parameter matrix 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        [Pure]
        public Matrix Dot(Matrix n)
        {
            if (_cols != n._rows)
                throw new Exception("Can not produce dot product, cols and rows not compatible");

            var f = new float[_rows, n._cols];

            //for each spot in the new matrix
            for (int i = 0; i < this._rows; i++)
            {
                for (int j = 0; j < n._cols; j++)
                {
                    float sum = 0;
                    for (int k = 0; k < this._cols; k++)
                    {
                        sum += _matrix[i, k] * n._matrix[k, j];
                    }
                    f[i, j] = sum;
                }
            }
            return new Matrix(f);
        }

        /// <summary>
        /// create a randomized matrix
        /// </summary>
        public static Matrix Random(int rows, int cols)
        {
            var r = new Random();
            var f = new float[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int sign = r.Next(-1, 1);
                    if (sign == 0)
                    {
                        sign = 1;
                    }
                    f[i, j] = (float) (sign * r.NextDouble());
                }
            }
            return new Matrix(f);
        }
        
        /// <summary>
        /// returns an array which represents this matrix
        /// elements are listed linearly by rows
        /// </summary>
        /// <returns></returns>
        public float[] ToArray()
        {
            float[] result = new float[_rows * _cols];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    result[j + i * _cols] = _matrix[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// for n x 1 matrices adds one to the bottom
        /// </summary>
        /// <returns></returns>
        public Matrix AddBias()
        {
            var f = new float[_rows + 1, 1];
            for (int i = 0; i < _rows; i++)
            {
                f[i, 0] = _matrix[i, 0];
            }
            f[_rows, 0] = 1;
            return new Matrix(f);
        }

        /// <summary>
        /// applies the activation function(sigmoid) to each element of the matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Activate()
        {
            var result = new float[_rows, _cols];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    var x = _matrix[i, j];
                    var sigmoid = (float)(1 / (1 + Math.Pow(Math.E, -x)));
                    result[i, j] = sigmoid;
                }
            }
            return new Matrix(result);
        }

        /// <summary>
        /// Mutation function for genetic algorithm 
        /// </summary>
        /// <param name="mutationRate"></param>
        [Pure]
        public Matrix Mutate(float mutationRate)
        {
            var r = new Random();
            var result = new float[_rows, _cols];
            //for each element in the matrix
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    float rand = (float) r.NextDouble();
                    if (rand < mutationRate)
                    {
                        //if chosen to be mutated
                        var mutation = _matrix[i,j] + ((float)r.NextGaussian() / 5f);//add a random value to it(can be negative)

                        //clamp the boundaries to 1 and -1
                        if( mutation > 1)
                        {
                            mutation = 1;
                        }
                        if (mutation < -1)
                        {
                            mutation = -1;
                        }
                        result[i, j] = mutation;
                    }
                }
            }
            return new Matrix(result);
        }

        /// <summary>
        /// returns a matrix which has a random number of vaules from this matrix and the rest from the parameter matrix
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        [Pure]
        public Matrix Crossover(Matrix partner)
        {
            var child = new float[_rows, _cols];
            var r = new Random();
            //pick a random point in the matrix
            int randC = r.Next(_cols);
            int randR = r.Next(_rows);
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (i < randR || i == randR && j <= randC)
                    { 
                        //if before the random point then copy from this matrix
                        child[i, j] = _matrix[i, j];
                    }
                    else
                    { 
                        //if after the random point then copy from the parameter array
                        child[i, j] = partner._matrix[i, j];
                    }
                }
            }
            return new Matrix(child);
        }

        /// <summary>
        /// return a copy of this matrix
        /// </summary>
        /// <returns></returns>
        [Pure]
        public Matrix Clone()
        {
            var clone = new float[_rows, _cols];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    clone[i, j] = _matrix[i, j];
                }
            }
            return new Matrix(clone);
        }
    }
}
