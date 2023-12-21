namespace ConsoleToy.Core;

public readonly record struct Cell(
    ConsoleColor Foreground,
    ConsoleColor Background,
    char Symbol)
{
    public static Cell Empty { get; } = new(ConsoleColor.White, ConsoleColor.Black, ' ');
}