namespace ConsoleToy.Core;

public interface IToy
{
    void Start(Canvas<Cell> canvas);

    ToyUpdateResult Update(Canvas<Cell> canvas, ConsoleKeyInfo? input = null);
}