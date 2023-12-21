using ConsoleToy.Core;

namespace ConsoleToy;

public sealed class ToySelector
{
    private readonly Func<IToy>[] _toysFactories;
    private int _currentToyIndex;

    public ToySelector(Func<IToy>[] toysFactories)
    {
        _toysFactories = toysFactories;
    }

    public IToy Next()
    {
        _currentToyIndex = (_currentToyIndex + 1) % _toysFactories.Length;
        return _toysFactories[_currentToyIndex]();
    }

    public IToy Previous()
    {
        _currentToyIndex = Math.Abs(_currentToyIndex - 1) % _toysFactories.Length;
        return _toysFactories[_currentToyIndex]();
    }
}