namespace MatrixOperations
{
	class Matrix
	{
		/// <summary>
		/// Integer that defines row's count of matrix
		/// </summary>
		private int _rows;

		/// <summary>
		/// Integer that defines column's count of matrix
		/// </summary>
		private int _columns;

		public Matrix(int rows, int columns)
		{
			Rows = rows;
			Columns = columns;
		}

		/// <summary>
		/// Integer that defines row's count of matrix
		/// </summary>
		public int Rows
		{
			get { return _rows; }

			set { _rows = value > 0 ? value : 0; }
		}

		/// <summary>
		/// Integer that defines column's count of matrix
		/// </summary>
		public int Columns
		{
			get { return _columns; }

			set { _columns = value > 0 ? value : 0; }
		}
	}
}
