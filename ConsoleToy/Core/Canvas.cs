namespace ConsoleToy.Core;

public sealed class Canvas<T>
{
    private static readonly EqualityComparer<T> DefaultComparer = EqualityComparer<T>.Default;

    private readonly T[,] _current;
    private readonly T[,] _next;

    public Canvas(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;

        _current = new T[rows, columns];
        _next = new T[rows, columns];
    }

    public int Rows { get; }

    public int Columns { get; }

    public void Fill(T symbol)
    {
        FillCurrent(symbol);
        FillNext(symbol);
    }

    private void FillNext(T symbol)
    {
        for (var i = 0; i < _next.GetLength(0); i++)
        for (var j = 0; j < _next.GetLength(1); j++)
        {
            _next[i, j] = symbol;
        }
    }

    private void FillCurrent(T symbol)
    {
        for (var i = 0; i < _current.GetLength(0); i++)
        for (var j = 0; j < _current.GetLength(1); j++)
        {
            _current[i, j] = symbol;
        }
    }

    public DiffEnumerator GetDifference()
    {
        return new DiffEnumerator(_current, _next);
    }

    public VerticalDiffEnumerator GetVerticalDifference()
    {
        return new VerticalDiffEnumerator(_current, _next);
    }

    public ref T GetCurrent(int i, int j)
    {
        return ref _current[i, j];
    }

    public void Patch(Point2D point, T value)
    {
        _current[point.RowIndex, point.ColumnIndex] = value;
    }

    public ref T this[int i, int j] => ref _next[i, j];

    public ref T this[Point2D point] => ref _next[point.RowIndex, point.ColumnIndex];

    public struct VerticalDiffEnumerator
    {
        private readonly T[,] _current;
        private readonly T[,] _next;
        private int _j;
        private int _i;

        public VerticalDiffEnumerator(T[,] current, T[,] next)
        {
            _current = current;
            _next = next;
        }

        public VerticalDiffEnumerator GetEnumerator() => this;

        public (Point2D, T) Current { get; private set; }
        
        public bool MoveNext()
        {
            while (_j < _current.GetLength(1))
            {
                while (_i < _current.GetLength(0))
                {
                    if (DefaultComparer.Equals(_current[_i, _j], _next[_i, _j]))
                    {
                        _i++;
                        continue;
                    }
                    Current = ((_i, _j), _next[_i, _j]);
                    _i++;
                    return true;
                }

                _j++;
                _i = 0;
            }

            return false;
        }
    }
    
    public struct DiffEnumerator
    {
        private readonly T[,] _current;
        private readonly T[,] _next;
        private int _i;
        private int _j;

        public DiffEnumerator(T[,] current, T[,] next)
        {
            _current = current;
            _next = next;
        }

        public DiffEnumerator GetEnumerator() => this;

        public (Point2D, T) Current { get; private set; }
        
        public bool MoveNext()
        {
            while (_i < _current.GetLength(0))
            {
                while (_j < _current.GetLength(1))
                {
                    if (DefaultComparer.Equals(_current[_i, _j], _next[_i, _j]))
                    {
                        _j++;
                        continue;
                    }
                    Current = ((_i, _j), _next[_i, _j]);
                    _j++;
                    return true;
                }

                _i++;
                _j = 0;
            }

            return false;
        }
    }
}