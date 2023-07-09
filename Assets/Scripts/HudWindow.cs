using TMPro;
using UnityEngine;

public class HudWindow : MonoBehaviour
{
    [SerializeField] private GameObject view;
    
    private void OnEnable()
    {
        GlobalEvents.GameStateChanged.AddListener(OnGameStateChanged);
    }
    private void OnDisable()
    {
        GlobalEvents.GameStateChanged.AddListener(OnGameStateChanged);
    }
    
    private void OnGameStateChanged(GameState newState)
    {
        view.SetActive(newState is FreeState);
    }
}