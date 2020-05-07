using System;
using System.Collections;
using System.Collections.Generic;

namespace MatrixOperations
{
	public class Matrix : IEnumerable<double>
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
	}
}
