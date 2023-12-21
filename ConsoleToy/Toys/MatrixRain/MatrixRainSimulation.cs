using ConsoleToy.Core;

namespace ConsoleToy.Toys.MatrixRain;

public sealed class MatrixRainSimulation : IToy
{
    private readonly MatrixRainSimulationOptions _options;

    public MatrixRainSimulation(MatrixRainSimulationOptions options)
    {
        _options = options;
    }

    public void Start(ref Canvas<Cell> canvas)
    {
    }

    public ToyUpdateResult Update(ref Canvas<Cell> canvas, ConsoleKeyInfo? input = null)
    {
        return ToyUpdateResult.Ok;
    }
}