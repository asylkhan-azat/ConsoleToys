using ConsoleToy.Core;

namespace ConsoleToy.Toys.MatrixRain;

public sealed class MatrixRainSimulation : IToy
{
    private readonly MatrixRainSimulationOptions _options;

    private float[]? _speeds;

    public MatrixRainSimulation(MatrixRainSimulationOptions? options = null)
    {
        _options = options ?? new MatrixRainSimulationOptions();
    }

    public void Start(Canvas<Cell> canvas)
    {
        _speeds = new float[canvas.Columns];

        for (var i = 0; i < canvas.Columns; i++)
        {
            _speeds[i] = _options.Random.NextSingle() / 2 + 0.225F;
        }
    }

    public ToyUpdateResult Update(Canvas<Cell> canvas, ConsoleKeyInfo? input = null)
    {
        StartOrCutWaves(canvas);
        MoveDown(canvas);
        
        if (input.HasValue)
        {
            HandleInput(canvas, input.Value);
        }
        
        return ToyUpdateResult.Ok;
    }

    private void HandleInput(Canvas<Cell> canvas, ConsoleKeyInfo input)
    {
        switch (input.Key)
        {
            case ConsoleKey.NumPad0:
                CreateExplosion(canvas, 1F);
                break;
            case ConsoleKey.NumPad1:
                CreateExplosion(canvas, 0.1F);
                break;
            case ConsoleKey.NumPad2:
                CreateExplosion(canvas, 0.2F);
                break;
            case ConsoleKey.NumPad3:
                CreateExplosion(canvas, 0.3F);
                break;
            case ConsoleKey.NumPad4:
                CreateExplosion(canvas, 0.4F);
                break;
            case ConsoleKey.NumPad5:
                CreateExplosion(canvas, 0.5F);
                break;
            case ConsoleKey.NumPad6:
                CreateExplosion(canvas, 0.6F);
                break;
            case ConsoleKey.NumPad7:
                CreateExplosion(canvas, 0.7F);
                break;
            case ConsoleKey.NumPad8:
                CreateExplosion(canvas, 0.8F);
                break;
            case ConsoleKey.NumPad9:
                CreateExplosion(canvas, 0.9F);
                break;
        }
    }

    private void CreateExplosion(Canvas<Cell> canvas, float power)
    {
        var (i, j) = (_options.Random.Next() % canvas.Rows, _options.Random.Next() % canvas.Columns);
        var width = _options.Random.Next(_options.MinExplosionWidth, _options.MaxExplosionWidth);
        var height = _options.Random.Next(_options.MinExplosionHeight, _options.MaxExplosionHeight);
        var cells = (int)(width * height * power);

        while (cells-- > 0)
        {
            var y = Math.Abs(_options.Random.Next(i - height / 2, i + height / 2));
            var x = Math.Abs(_options.Random.Next(j - width / 2, j + width / 2));

            if (y < canvas.Rows && x < canvas.Columns)
            {
                canvas[(y, x)] = new Cell(
                    ConsoleColor.Red,
                    ConsoleColor.Black,
                    '*');
            }
        }
    }

    private void MoveDown(Canvas<Cell> canvas)
    {
        for (var j = 0; j < canvas.Columns; j++)
        {
            var speed = _speeds?[j] ?? 0.1F;

            if (_options.Random.NextSingle() * speed < _options.SlowDownChance)
            {
                continue;
            }

            for (var i = 0; i < canvas.Rows - 1; i++)
            {
                var current = canvas.GetCurrent(i, j);

                canvas[i + 1, j] = (i + 1) % 6 == 0
                    ? current with
                    {
                        Foreground = current.Foreground switch
                        {
                            ConsoleColor.DarkGreen => ConsoleColor.Green,
                            ConsoleColor.Green => ConsoleColor.DarkGreen,
                            _ => current.Foreground
                        }
                    }
                    : current;
            }
        }
    }

    private void StartOrCutWaves(Canvas<Cell> canvas)
    {
        for (var i = 0; i < canvas.Columns; i++)
        {
            var currentCell = canvas.GetCurrent(0, i);

            if (currentCell.Symbol is not ' ')
            {
                canvas[0, i] = PutOrCut();
                continue;
            }

            if (ShouldPutNew())
            {
                canvas[0, i] = _options.CellGenerator(true);
            }
        }
    }

    private bool ShouldPutNew()
    {
        return _options.Random.NextSingle() < _options.WaveStartChance;
    }

    private Cell PutOrCut()
    {
        if (ShouldCutWave())
        {
            return Cell.Empty;
        }

        return _options.CellGenerator(false);
    }

    private bool ShouldCutWave()
    {
        return _options.Random.NextSingle() < _options.WaveCutChance;
    }
}