using UnityEngine.Events;

public static class GlobalEvents
{
    public static UnityEvent             Interaction          = new();
    public static UnityEvent<Slot, Slot> InterSlotExchange   = new();
    public static UnityEvent<GameState>  GameStateChanged    = new();
}