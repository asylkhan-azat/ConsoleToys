using ConsoleToy.Core;

namespace ConsoleToy.Drawing;

public static class ConsoleDrawer
{
    public static void Draw(Canvas<Cell> canvas, bool renderVertically)
    {
        if (renderVertically)
        {
            DrawVertically(canvas);
            return;
        }

        var oldForeground = Console.ForegroundColor;
        var oldBackground = Console.BackgroundColor;

        foreach (var (point, cell) in canvas.GetDifference())
        {
            Console.SetCursorPosition(point.ColumnIndex, point.RowIndex);

            Write(ref oldForeground, ref oldBackground, cell);
            canvas.Patch(point, cell);
        }
    }

    private static void DrawVertically(Canvas<Cell> canvas)
    {
        var oldForeground = Console.ForegroundColor;
        var oldBackground = Console.BackgroundColor;

        foreach (var (point, cell) in canvas.GetVerticalDifference())
        {
            Console.SetCursorPosition(point.ColumnIndex, point.RowIndex);
            Write(ref oldForeground, ref oldBackground, cell);
            canvas.Patch(point, cell);
        }
    }

    private static void Write(
        ref ConsoleColor oldForeground,
        ref ConsoleColor oldBackground,
        Cell cell)
    {
        if (oldForeground != cell.Foreground)
        {
            Console.ForegroundColor = cell.Foreground;
        }

        if (oldBackground != cell.Background)
        {
            Console.BackgroundColor = cell.Background;
        }

        Console.Write(cell.Symbol);

        oldForeground = cell.Foreground;
        oldBackground = cell.Background;
    }
}