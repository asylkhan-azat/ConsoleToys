namespace ConsoleToy.Core;

public readonly struct Canvas<T>
{
    private readonly ArrayBacked2DMatrix<T> _current;
    private readonly ArrayBacked2DMatrix<T> _next;

    public Canvas(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;

        _current = new ArrayBacked2DMatrix<T>(rows, columns);
        _next = new ArrayBacked2DMatrix<T>(rows, columns);
    }

    public int Rows { get; }

    public int Columns { get; }

    public void FillCurrent(T symbol)
    {
        _current.Fill(symbol);
    }

    public void FillNext(T symbol)
    {
        _next.Fill(symbol);
    }

    public void Fill(T symbol)
    {
        FillCurrent(symbol);
        FillNext(symbol);
    }

    public ArrayBacked2DMatrix<T>.DiffEnumerator GetDifference()
    {
        return _current.Diff(_next);
    }

    public void Synchronize()
    {
        _next.CopyTo(_current);
    }

    public ref T this[int i, int j] => ref _next[i, j];

    public ref T this[Point2D point] => ref _next[point];
}