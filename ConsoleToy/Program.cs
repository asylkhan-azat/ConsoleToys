using System.Runtime.InteropServices;
using ConsoleToy.Core;
using ConsoleToy.Drawing;
using ConsoleToy.Toys.GameOfLife;


Init();

var (rows, columns) = (Console.WindowHeight, Console.WindowWidth - 1);

var toy = new ConwaysGameOfLife();
var canvas = new Canvas<Cell>(rows, columns);

toy.Start(ref canvas);
ConsoleDrawer.Draw(ref canvas);

while (true)
{
    Thread.Sleep(15);

    if (toy.Update(ref canvas) != ToyUpdateResult.Ok)
    {
        toy.Start(ref canvas);
    }

    ConsoleDrawer.Draw(ref canvas);
}

return;

static void Init()
{
    ResizeConsoleIfPossible();
}

static void ResizeConsoleIfPossible()
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        Console.BufferHeight = Console.WindowHeight;
        Console.BufferWidth = Console.WindowWidth;
    }
}