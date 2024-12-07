using ConsoleToy.Core;

namespace ConsoleToy.Toys.MatrixRain;

public class MatrixRainSimulationOptions
{
    public MatrixRainSimulationOptions()
    {
        const string symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ012345679";

        var counter = 0;
        
        CellGenerator = () =>
        {
            var symbol = symbols[++counter % symbols.Length];

            var color = (counter % 7) switch
            {
                >= 4 => ConsoleColor.Green,
                _ => ConsoleColor.DarkGreen
            };

            return new Cell(
                color,
                ConsoleColor.Black,
                symbol);
        };
    }

    public Func<Cell> CellGenerator { get; set; }

    public Random Random { get; set; } = new();

    public float WaveCutChance { get; set; } = 0.275F;

    public float WaveStartChance { get; set; } = 0.07F;

    public float SlowDownChance { get; set; } = 0.08F;

    public int MinExplosionWidth { get; set; } = 7;

    public int MaxExplosionWidth { get; set; } = 27;

    public int MinExplosionHeight { get; set; } = 6;

    public int MaxExplosionHeight { get; set; } = 16;
}