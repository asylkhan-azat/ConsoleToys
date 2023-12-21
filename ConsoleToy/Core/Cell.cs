namespace ConsoleToy.Core;

public readonly record struct Cell(
    ConsoleColor Foreground,
    ConsoleColor Background,
    char Symbol);