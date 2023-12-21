using ConsoleToy.Core;

namespace ConsoleToy.Toys.MatrixRain;

public class MatrixRainSimulationOptions
{
    private int _counter;

    public MatrixRainSimulationOptions()
    {
        const string symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ012345679";

        CellGenerator = start =>
        {
            var symbol = symbols[++_counter % symbols.Length];

            var color = (_counter % 7) switch
            {
                >= 4 => ConsoleColor.Green,
                _ => start && _counter % 7 == 0 ? ConsoleColor.White : ConsoleColor.DarkGreen
            };

            return new Cell(
                color,
                ConsoleColor.Black,
                symbol);
        };
    }

    public Func<bool, Cell> CellGenerator { get; set; }

    public Random Random { get; set; } = new();

    public float WaveCutChance { get; set; } = 0.275F;

    public float WaveStartChance { get; set; } = 0.07F;

    public float SlowDownChance { get; set; } = 0.08F;

    public int MinExplosionWidth { get; set; } = 7;

    public int MaxExplosionWidth { get; set; } = 27;

    public int MinExplosionHeight { get; set; } = 6;

    public int MaxExplosionHeight { get; set; } = 16;
}