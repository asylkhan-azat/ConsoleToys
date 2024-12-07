using System.Diagnostics;
using System.Runtime.InteropServices;
using ConsoleToy.Core;

namespace ConsoleToy.Toys.GameOfLife;

public sealed class ConwaysGameOfLife : IToy
{
    private readonly Queue<bool[,]> _previousStates = new();

    private readonly ConwaysGameOfLifeOptions _options;

    private bool[,]? _current;
    private bool[,]? _next;

    public ConwaysGameOfLife(ConwaysGameOfLifeOptions? options = null)
    {
        _options = options ?? new ConwaysGameOfLifeOptions();
    }

    public void Start(Canvas<Cell> canvas)
    {
        _current = new bool[canvas.Rows, canvas.Columns];
        _next = new bool[canvas.Rows, canvas.Columns];

        PopulateWithAliveCells(canvas);
    }

    public ToyUpdateResult Update(Canvas<Cell> canvas, ConsoleKeyInfo? input = null)
    {
        Debug.Assert(_current is not null && _next is not null);

        if (input.HasValue)
        {
            HandleInput(canvas, input.Value);
        }

        UpdateNext(canvas);

        if (TryDetectCycle())
        {
            return ToyUpdateResult.End;
        }

        Array.Copy(_next, _current, _next.Length);
        return ToyUpdateResult.Ok;
    }

    private bool TryDetectCycle()
    {
        Debug.Assert(_current is not null && _next is not null);

        if (_options.CycleDetectionQueueSize is 0)
        {
            return false;
        }

        var cycled = false;

        _previousStates.Enqueue((bool[,])_current.Clone());

        foreach (var previousState in _previousStates)
        {
            var next = MemoryMarshal.CreateSpan(
                ref MemoryMarshal.GetArrayDataReference(_next), _next.Length);

            var previous = MemoryMarshal.CreateSpan(
                ref MemoryMarshal.GetArrayDataReference(previousState), previousState.Length);

            if (!next.SequenceEqual(previous)) continue;
            cycled = true;
            break;
        }

        if (_previousStates.Count > _options.CycleDetectionQueueSize)
        {
            _previousStates.Dequeue();
        }

        return cycled;
    }

    private void PopulateWithAliveCells(Canvas<Cell> canvas)
    {
        Debug.Assert(_current is not null && _next is not null);

        var cellsToCreate = (int)(canvas.Rows * canvas.Columns * _options.InitialCellsPopulationRatio);

        while (cellsToCreate > 0)
        {
            Point2D point = (_options.Random.Next(_current.GetLength(0)), _options.Random.Next(_current.GetLength(1)));

            if (IsAlive(point)) continue;

            _current[point.RowIndex, point.ColumnIndex] = true;
            canvas[point] = _options.AliveCellSymbolFactory();
            cellsToCreate--;
        }
    }

    private void UpdateNext(Canvas<Cell> canvas)
    {
        Debug.Assert(_current is not null && _next is not null);

        for (var i = 0; i < canvas.Rows; i++)
        {
            for (var j = 0; j < canvas.Columns; j++)
            {
                Point2D point = (i, j);

                var alive = IsAliveOnNext(point);
                _next[point.RowIndex, point.ColumnIndex] = alive;
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
        Debug.Assert(_current is not null && _next is not null);

        if (point.RowIndex >= 0 &&
            point.RowIndex < _next.GetLength(0) &&
            point.ColumnIndex >= 0 &&
            point.ColumnIndex < _next.GetLength(1))
        {
            return _current[point.RowIndex, point.ColumnIndex];
        }

        return false;
    }

    private void HandleInput(Canvas<Cell> canvas, ConsoleKeyInfo input)
    {
        switch (input.Key)
        {
            case ConsoleKey.OemMinus:
                _options.CycleDetectionQueueSize = Math.Max(0, _options.CycleDetectionQueueSize - 1);
                break;

            case ConsoleKey.OemPlus:
                _options.CycleDetectionQueueSize++;
                break;

            case ConsoleKey.NumPad0:
                _options.InitialCellsPopulationRatio = 0.025F;
                Start(canvas);
                break;

            case ConsoleKey.NumPad1:
                _options.InitialCellsPopulationRatio = 0.050F;
                Start(canvas);
                break;

            case ConsoleKey.NumPad2:
                _options.InitialCellsPopulationRatio = 0.075F;
                Start(canvas);
                break;
            case ConsoleKey.NumPad3:
                _options.InitialCellsPopulationRatio = 0.100F;
                Start(canvas);
                break;
            case ConsoleKey.NumPad4:
                _options.InitialCellsPopulationRatio = 0.125F;
                Start(canvas);
                break;
            case ConsoleKey.NumPad5:
                Start(canvas);
                _options.InitialCellsPopulationRatio = 0.150F;
                Start(canvas);
                break;
            case ConsoleKey.NumPad6:
                _options.InitialCellsPopulationRatio = 0.175F;
                Start(canvas);
                break;
            case ConsoleKey.NumPad7:
                _options.InitialCellsPopulationRatio = 0.200F;
                Start(canvas);
                break;
            case ConsoleKey.NumPad8:
                _options.InitialCellsPopulationRatio = 0.225F;
                Start(canvas);
                break;
            case ConsoleKey.NumPad9:
                _options.InitialCellsPopulationRatio = 0.250F;
                Start(canvas);
                break;
        }
    }
}