using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private Character target;

    [Space]
    [SerializeField] private GameObject view;
    [SerializeField] private TMP_Text label;
    
    private void OnEnable()
    {
        GlobalEvents.GameStateChanged.AddListener(OnGameStateChanged);
        target.MoneyChanged.AddListener(UpdateDisplay);
        UpdateDisplay(target.Money);
    }
    private void OnDisable()
    {
        GlobalEvents.GameStateChanged.AddListener(OnGameStateChanged);
        target.MoneyChanged.RemoveListener(UpdateDisplay);
    }
    
    private void OnGameStateChanged(GameState newState)
    {
        view.SetActive(newState is FreeState);
    }

    private void UpdateDisplay(int arg0)
    {
        label.text = arg0.ToString();
    }
}