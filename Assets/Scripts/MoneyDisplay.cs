using System;
using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private Character target;

    [Space]
    [SerializeField] private TMP_Text label;
    
    private void OnEnable()
    {
        target.MoneyChanged.AddListener(UpdateDisplay);
        UpdateDisplay(target.Money);
    }
    private void OnDisable()
    {
        target.MoneyChanged.RemoveListener(UpdateDisplay);
    }

    private void UpdateDisplay(int arg0)
    {
        label.text = arg0.ToString();
    }

    private void OnValidate()
    {
        if (!label)
        {
            label = GetComponent<TMP_Text>();
        }
    }
}