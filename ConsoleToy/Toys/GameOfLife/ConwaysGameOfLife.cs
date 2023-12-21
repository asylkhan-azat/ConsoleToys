using ConsoleToy.Core;

namespace ConsoleToy.Toys.GameOfLife;

public sealed class ConwaysGameOfLife : IToy
{

    private readonly Queue<ArrayBacked2DMatrix<bool>> _previousStates = new();

    private readonly ConwaysGameOfLifeOptions _options;

    private ArrayBacked2DMatrix<bool> _current;
    private ArrayBacked2DMatrix<bool> _next;

    public ConwaysGameOfLife(ConwaysGameOfLifeOptions? options = null)
    {
        _options = options ?? new ConwaysGameOfLifeOptions();
    }

    public void Start(ref Canvas<Cell> canvas)
    {
        _current = new ArrayBacked2DMatrix<bool>(canvas.Rows, canvas.Columns);
        _next = new ArrayBacked2DMatrix<bool>(canvas.Rows, canvas.Columns);

        PopulateWithAliveCells(ref canvas);
    }

    public ToyUpdateResult Update(ref Canvas<Cell> canvas, ConsoleKey? input = null)
    {
        if (input.HasValue)
        {
            HandleInput(input.Value);
        }

        UpdateNext(ref canvas);

        if (TryDetectCycle())
        {
            return ToyUpdateResult.End;
        }

        _next.CopyTo(_current);
        return ToyUpdateResult.Ok;
    }

    private bool TryDetectCycle()
    {
        if (_options.CycleDetectionQueueSize is 0)
        {
            return false;
        }

        _previousStates.Enqueue(_current.Clone());

        foreach (var previousState in _previousStates)
        {
            if (previousState.SequenceEqual(_next))
            {
                return true;
            }
        }

        if (_previousStates.Count >= _options.CycleDetectionQueueSize)
        {
            _previousStates.Dequeue();
        }

        return false;
    }

    private void PopulateWithAliveCells(ref Canvas<Cell> canvas)
    {
        var alive = 0;

        while (alive < _options.InitialCellsPopulation)
        {
            Point2D point = (_options.Random.Next(_current.Rows), _options.Random.Next(_current.Columns));

            if (!IsAlive(point))
            {
                _current[point] = true;
                canvas[point] = _options.AliveCellSymbolFactory();
                alive++;
            }
        }
    }

    private void UpdateNext(ref Canvas<Cell> canvas)
    {
        for (var i = 0; i < canvas.Rows; i++)
        {
            for (var j = 0; j < canvas.Columns; j++)
            {
                Point2D point = (i, j);

                var alive = IsAliveOnNext(point);

                _next[point] = alive;

                canvas[point] = alive ? _options.AliveCellSymbolFactory() : _options.DeadCellSymbolFactory();
            }
        }
    }

    private bool IsAliveOnNext(Point2D point)
    {
        var aliveNeighbours = CountAliveNeighbours(point);

        if (IsAlive(point))
        {
            return aliveNeighbours is 2 or 3;
        }

        return aliveNeighbours is 3;
    }

    private int CountAliveNeighbours(Point2D point)
    {
        var count = 0;

        if (IsAlive(point.Top())) count++;
        if (IsAlive(point.LeftTop())) count++;
        if (IsAlive(point.RightTop())) count++;
        if (IsAlive(point.Left())) count++;
        if (IsAlive(point.Right())) count++;
        if (IsAlive(point.Bottom())) count++;
        if (IsAlive(point.LeftBottom())) count++;
        if (IsAlive(point.RightBottom())) count++;

        return count;
    }

    private bool IsAlive(Point2D point)
    {
        if (point.RowIndex >= 0 &&
            point.RowIndex < _next.Rows &&
            point.ColumnIndex >= 0 &&
            point.ColumnIndex < _next.Columns)
        {
            return _current[point];
        }

        return false;
    }

    private void HandleInput(ConsoleKey input)
    {
        switch (input)
        {
            case ConsoleKey.LeftArrow:
                break;

            case ConsoleKey.RightArrow:
                break;
        }
    }
}