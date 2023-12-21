using ConsoleToy.Core;

namespace ConsoleToy.Toys.GameOfLife;

public sealed class ConwaysGameOfLifeOptions
{
    public Random Random { get; set; } = Random.Shared;

    public float InitialCellsPopulationRatio { get; set; } = 0.125F;
    
    public int CycleDetectionQueueSize { get; set; } = 5;

    public Func<Cell> DeadCellSymbolFactory { get; set; } =
        static () => new Cell(ConsoleColor.Black, ConsoleColor.Black, ' ');

    public Func<Cell> AliveCellSymbolFactory { get; set; } =
        static () => new Cell(ConsoleColor.Green, ConsoleColor.Black, '\u2022');
}