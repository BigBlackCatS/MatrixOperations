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

			var inverse = new Matrix(Rows, Columns);

			for (int i = 0; i < Rows; i++)
				inverse[i, i] = 1;

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
