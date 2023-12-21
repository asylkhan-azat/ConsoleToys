namespace ConsoleToy.Core;

public interface IToy
{
    void Start(ref Canvas<Cell> canvas);

    ToyUpdateResult Update(ref Canvas<Cell> canvas, ConsoleKeyInfo? input = null);
}