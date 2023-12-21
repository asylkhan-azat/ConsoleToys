using System.Text;
using ConsoleToy;
using ConsoleToy.Core;
using ConsoleToy.Drawing;
using ConsoleToy.Toys.GameOfLife;
using ConsoleToy.Toys.MatrixRain;

Console.OutputEncoding = Encoding.UTF8;

var toySelector = new ToySelector([
    static () => new MatrixRainSimulation(),
    static () => new ConwaysGameOfLife(),
]);

var renderVertically = false;
IToy toy;
var tick = 10;

var canvas = CaptureCanvas();

while (true)
{
    OnGameChange();
    toy = toySelector.Next();
    toy.Start(canvas);

    try
    {
        while (true)
        {
            var input = GetInput();

            if (input.HasValue && HandleInput(input.Value))
            {
                continue;
            }

            ConsoleDrawer.Draw(canvas, renderVertically);

            if (toy.Update(canvas, input) != ToyUpdateResult.Ok)
            {
                toy.Start(canvas);
            }

            if (tick > 0)
            {
                Thread.Sleep(tick);
            }
        }
    }
    catch (Exception)
    {
        // Error was not caused because of window resize
        if ((Console.WindowHeight, Console.WindowWidth - 1) == (canvas.Rows, canvas.Columns))
        {
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
    }
    finally
    {
        if ((Console.WindowHeight, Console.WindowWidth - 1) != (canvas.Rows, canvas.Columns))
        {
            canvas = CaptureCanvas();
        }
    }
}

bool HandleInput(ConsoleKeyInfo input)
{
    switch (input.Key)
    {
        case ConsoleKey.Enter:
            renderVertically = !renderVertically;
            return true;
        
        case ConsoleKey.LeftArrow:
            OnGameChange();
            toy = toySelector.Previous();
            toy.Start(canvas);
            return true;

        case ConsoleKey.RightArrow:
            OnGameChange();
            toy = toySelector.Next();
            toy.Start(canvas);
            return true;

        case ConsoleKey.UpArrow:
            tick = Math.Min(tick + 10, 1000);
            return true;

        case ConsoleKey.DownArrow:
            tick = Math.Max(tick - 10, 0);
            return true;

        default:
            return false;
    }
}

static ConsoleKeyInfo? GetInput()
{
    if (Console.KeyAvailable)
    {
        return Console.ReadKey();
    }

    return null;
}

void OnGameChange()
{
    Console.Clear();
    
    Console.CursorVisible = false;

    if ((Console.WindowHeight, Console.WindowWidth - 1) != (canvas.Rows, canvas.Columns))
    {
        canvas = CaptureCanvas();
    }
}

static Canvas<Cell> CaptureCanvas()
{
    var (rows, columns) = (Console.WindowHeight, Console.WindowWidth - 1);
    var canvas = new Canvas<Cell>(rows, columns);
    canvas.Fill(Cell.Empty);
    return canvas;
}