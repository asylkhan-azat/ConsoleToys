namespace ConsoleToy.Core;

public readonly struct ArrayBacked2DMatrix<T>
{
    private readonly T[] _items;

    public ArrayBacked2DMatrix(int rows, int columns)
    {
        _items = new T[SizeFor(rows, columns)];

        Rows = rows;
        Columns = columns;
    }

    public int Rows { get; }

    public int Columns { get; }

    public ref T this[Point2D point] =>
        ref this[point.RowIndex, point.ColumnIndex];

    public ref T this[int rowIndex, int columnIndex] =>
        ref _items[GetOneDimensionalIndex(rowIndex, columnIndex, Columns)];

    public void Fill(T value)
    {
        _items.AsSpan().Fill(value);
    }

    public void CopyTo(ArrayBacked2DMatrix<T> other)
    {
        _items.AsSpan().CopyTo(other._items.AsSpan());
    }

    public DiffEnumerator Diff(ArrayBacked2DMatrix<T> other)
    {
        return new DiffEnumerator(
            _items,
            other._items,
            Columns);
    }

    public void Clear()
    {
        Array.Clear(_items);
    }

    public bool SequenceEqual(ArrayBacked2DMatrix<T> other)
    {
        return _items.AsSpan().SequenceEqual(other._items);
    }

    public ArrayBacked2DMatrix<T> Clone()
    {
        var clone = new ArrayBacked2DMatrix<T>(Rows, Columns);
        CopyTo(clone);
        return clone;
    }

    private static int SizeFor(int rows, int columns)
    {
        return rows * columns;
    }

    public struct DiffEnumerator
    {
        private readonly T[] _left;
        private readonly T[] _right;
        private readonly int _columns;

        private int _i = -1;

        public DiffEnumerator(
            T[] left,
            T[] right,
            int columns)
        {
            _left = left;
            _right = right;
            _columns = columns;
        }

        public DiffEnumerator GetEnumerator() => this;

        public (Point2D, T) Current { get; private set; }

        public bool MoveNext()
        {
            while (++_i < _left.Length)
            {
                if (EqualityComparer<T>.Default.Equals(_left[_i], _right[_i])) continue;

                var (rowIndex, columnIndex) = GetTwoDimensionalIndices(_i, _columns);
                Current = (new Point2D(rowIndex, columnIndex), _right[_i]);
                return true;
            }

            return false;
        }
    }

    private static int GetOneDimensionalIndex(int rowIndex, int columnIndex, int columns)
    {
        return rowIndex * columns + columnIndex;
    }

    private static (int rowIndex, int columnIndex) GetTwoDimensionalIndices(int index, int columns)
    {
        return (index / columns, index % columns);
    }
}