using ConsoleToy.Core;

namespace ConsoleToy.Drawing;

public static class ConsoleDrawer
{
    public static void Draw(ref Canvas<Cell> canvas)
    {
        foreach (var (point, cell) in canvas.GetDifference())
        {
            Console.SetCursorPosition(point.ColumnIndex, point.RowIndex);

            if (Console.ForegroundColor != cell.Foreground)
            {
                Console.ForegroundColor = cell.Foreground;
            }

            if (Console.BackgroundColor != cell.Background)
            {
                Console.BackgroundColor = cell.Background;
            }

            Console.Write(cell.Symbol);
        }

        canvas.Synchronize();
    }
}