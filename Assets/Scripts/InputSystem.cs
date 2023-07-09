using UnityEngine;

public class InputSystem : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (GameState.Current is FreeState)
                GameState.Current = new InventoryState();
            else if (GameState.Current is InventoryState)
                GameState.Current = new FreeState();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameState.Current is FreeState)
                GameState.Current = new QuitState();
            else 
                GameState.Current = new FreeState();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            GlobalEvents.Interaction.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            GameState.Current = new QuitState();
        }
        
    }
}