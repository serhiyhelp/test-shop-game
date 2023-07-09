public abstract class GameState 
{
    private static GameState _current = new FreeState();

    public static GameState Current
    {
        get => _current;
        set
        {
            _current = value;
            GlobalEvents.GameStateChanged.Invoke(value);
        }
    }
    
    public abstract bool MovementAllowed { get; }
}