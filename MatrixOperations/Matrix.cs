using System;
using System.Collections;
using System.Collections.Generic;

namespace MatrixOperations
{
	public class Matrix : IEnumerable<double>, ICloneable
	{
		/// <summary>
		/// Integer that defines row's count of matrix
		/// </summary>
		private int _rows;

		/// <summary>
		/// Integer that defines column's count of matrix
		/// </summary>
		private int _columns;

		/// <summary>
		/// Array's view of matrix
		/// </summary>
		private double[,] _matrix;

		public Matrix(int rows, int columns)
		{
			Rows = rows;
			Columns = columns;
			_matrix = new double[Rows, Columns];
		}

		/// <summary>
		/// Indexer helps use cell of matrix as array's element
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		public double this[int row, int column]
		{
			get => _matrix[row, column];
			set => _matrix[row, column] = value;
		}

		/// <summary>
		/// Integer that defines row's count of matrix
		/// </summary>
		public int Rows
		{
			get => _rows;
			private set => _rows = value > 0 ? value : 0;
		}

		/// <summary>
		/// Integer that defines column's count of matrix
		/// </summary>
		public int Columns
		{
			get => _columns;
			private set => _columns = value > 0 ? value : 0;
		}

		/// <summary>
		/// Find identity matrix of specified size
		/// </summary>
		/// <param name="size">Size of identity matrix</param>
		/// <returns></returns>
		public static Matrix GetIdentityMatrix(int size)
		{
			var matrix = new Matrix(size, size);

			for (int i = 0; i < size; i++)
			{
				matrix[i, i] = 1;
			}

			return matrix;
		}

		/// <summary>
		/// Find the inverse matrix to the instance matrix
		/// </summary>
		/// <exception cref="InvalidOperationException">Matrix is not square</exception>
		/// <returns></returns>
		public Matrix Inverse()
		{
			if (Rows != Columns)
			{
				throw new InvalidOperationException();
			}

			var matrix = (Matrix)Clone();

			double multi, dev;

			var inverse = GetIdentityMatrix(Rows);

			for (int i = 0; i < Rows; i++)
			{
				dev = matrix[i, i];

				for (int k = 0; k < Columns; k++)
				{
					inverse[i, k] /= dev;
					matrix[i, k] /= dev;
				}

				for (int j = 0; j < Rows; j++)
				{
					if (i != j)
					{
						multi = matrix[j, i];

						for (int k = 0; k < Columns; k++)
						{
							inverse[j, k] = -inverse[i, k] * multi + inverse[j, k];
							matrix[j, k] = -matrix[i, k] * multi + matrix[j, k];
						}
					}
				}
			}

			return inverse;
		}

		/// <summary>
		/// Find the determinant of the instance matrix
		/// </summary>
		/// <exception cref="InvalidOperationException">Matrix is not square</exception>
		/// <returns></returns>
		public double Determinant()
		{
			if (Rows != Columns)
			{
				throw new InvalidOperationException();
			}

			var matrix = (Matrix)Clone();
			double multi, dev;
			double det = 1;

			for (int i = 0; i < Rows; i++)
			{
				if (matrix[i, i] == 0)
				{
					var index = i == Rows - 1
						? i - 1
						: i + 1;

					for (int j = 0; j < Columns; j++)
					{
						double temp = -matrix[i, j];
						matrix[i, j] = matrix[index, j];
						matrix[index, j] = temp;
					}
				}
			}

			for (int i = 0; i < Rows; i++)
			{
				dev = matrix[i, i];

				for (int j = 0; j < Rows; j++)
				{
					if (i != j)
					{
						multi = matrix[j, i];

						for (int k = 0; k < Columns; k++)
						{
							matrix[j, k] = -matrix[i, k] / dev * multi + matrix[j, k];
						}
					}
				}
			}

			for (int i = 0; i < Rows; i++)
				det *= matrix[i, i];

			return det;
		}

		/// <summary>
		/// Find the sum of two matrices
		/// </summary>
		/// <param name="matrix1">First matrix</param>
		/// <param name="matrix2">Second matrix</param>
		/// <exception cref="InvalidOperationException">Dimension of the first matrix is not equal dimension of the second matrix</exception>
		/// <returns></returns>
		public static Matrix operator +(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.Rows != matrix2.Rows
					|| matrix1.Columns != matrix2.Columns)
			{
				throw new InvalidOperationException();
			}

			Matrix sum = new Matrix(matrix1.Rows, matrix1.Columns);
			for (int i = 0; i < sum.Rows; i++)
				for (int j = 0; j < sum.Columns; j++)
					sum[i, j] = matrix1[i, j] + matrix2[i, j];

			return sum;
		}

		/// <summary>
		/// Find the difference of two matrices
		/// </summary>
		/// <param name="matrix1">First matrix</param>
		/// <param name="matrix2">Second matrix</param>
		/// <exception cref="InvalidOperationException">Dimension of the first matrix is not equal dimension of the second matrix</exception>
		/// <returns></returns>
		public static Matrix operator -(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.Rows != matrix2.Rows
					|| matrix1.Columns != matrix2.Columns)
			{
				throw new InvalidOperationException();
			}

			Matrix sub = new Matrix(matrix1.Rows, matrix1.Columns);
			for (int i = 0; i < sub.Rows; i++)
				for (int j = 0; j < sub.Columns; j++)
					sub[i, j] = matrix1[i, j] - matrix2[i, j];

			return sub;
		}

		/// <summary>
		/// Find the product of two matrices
		/// </summary>
		/// <param name="matrix1">First matrix</param>
		/// <param name="matrix2">Second matrix</param>
		/// <exception cref="InvalidOperationException">Column count of the first matrix is not equal row count of the second matrix</exception>
		/// <returns></returns>
		public static Matrix operator *(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.Columns != matrix2.Rows)
			{
				throw new InvalidOperationException();
			}

			Matrix mul = new Matrix(matrix1.Rows, matrix2.Columns);

			for (int i = 0; i < mul.Rows; i++)
				for (int j = 0; j < mul.Columns; j++)
					for (int k = 0; k < matrix1.Columns; k++)
						mul[i, j] += matrix1[i, k] * matrix2[k, j];

			return mul;
		}

		/// <summary>
		/// Find the product matrix and numeric
		/// </summary>
		/// <param name="matrix">Matrix</param>
		/// <param name="numeric">Numeric</param>
		/// <returns></returns>
		public static Matrix operator *(Matrix matrix, double numeric)
		{
			Matrix mul = new Matrix(matrix.Rows, matrix.Columns);

			for (int i = 0; i < mul.Rows; i++)
				for (int j = 0; j < mul.Columns; j++)
					mul[i, j] = matrix[i, j] / numeric;

			return mul;
		}

		/// <summary>
		/// Find the division matrix to numeric
		/// </summary>
		/// <param name="matrix">Matrix</param>
		/// <param name="numeric">Numeric</param>
		/// <exception cref="DivideByZeroException">Numeric is 0</exception>
		/// <returns></returns>
		public static Matrix operator /(Matrix matrix, double numeric)
		{
			if (numeric == 0)
			{
				throw new DivideByZeroException();
			}

			Matrix mul = new Matrix(matrix.Rows, matrix.Columns);

			for (int i = 0; i < mul.Rows; i++)
				for (int j = 0; j < mul.Columns; j++)
					mul[i, j] = matrix[i, j] / numeric;

			return mul;
		}

		/// <summary>
		/// Find the transpose of a matrix
		/// </summary>
		/// <returns></returns>
		public Matrix T()
		{
			var t = new Matrix(Columns, Rows);

			for (int i = 0; i < t.Rows; i++)
				for (int j = 0; j < t.Columns; j++)
					t[i, j] = this[j, i];

			return t;
		}

		/// <summary>
		/// Convert matrix to two-dimensional array
		/// </summary>
		/// <returns></returns>
		public double[,] ToTwoDimArray()
		{
			var array = new double[Rows, Columns];

			for (int i = 0; i < array.GetLength(0); i++)
				for (int j = 0; j < array.GetLength(1); j++)
					array[i, j] = this[i, j];

			return array;
		}

		/// <summary>
		/// Convert row vector or column vector to array
		/// </summary>
		/// <returns></returns>
		public double[] ToArray()
		{
			if (Rows != 1 && Columns != 1)
			{
				throw new InvalidOperationException();
			}

			int arrayLength = Rows != 1 ? Rows : Columns;

			var array = new double[arrayLength];

			for (int i = 0; i < arrayLength; i++)
				array[i] = Rows != 1 ? this[i, 0] : this[0, i];

			return array;
		}

		/// <summary>
		/// Convert Matrix to number
		/// </summary>
		/// <returns></returns>
		public double ToNumber()
		{
			if (Rows != 1 || Columns != 1)
			{
				throw new InvalidOperationException();
			}

			return this[0, 0];
		}

		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

		public IEnumerator<double> GetEnumerator()
		{
			foreach (var cell in _matrix)
			{
				yield return cell;
			}
		}

		private int _lastRowIndex = 0;

		public void Add(params double[] row)
		{
			if (_lastRowIndex > Rows - 1)
			{
				throw new IndexOutOfRangeException();
			}

			for (int j = 0; j < row.Length; j++)
			{
				if (j > Columns - 1)
				{
					throw new IndexOutOfRangeException();
				}

				this[_lastRowIndex, j] = row[j];
			}

			_lastRowIndex++;
		}

	
		public object Clone()
		{
			var matrix = new Matrix(Rows, Columns);

			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					matrix[i, j] = this[i, j];
				}
			}

			return matrix;
		}
	}
}
