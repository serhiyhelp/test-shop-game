using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradingWindow : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
    
    private void ChangeViewVisibility(bool show)
    {
        _view.SetActive(show);
    }
}