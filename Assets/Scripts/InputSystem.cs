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
            GameState.Current = new FreeState();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            GlobalEvents.Interaction.Invoke();
        }
        
    }
}