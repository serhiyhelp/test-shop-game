using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ExitDialog : MonoBehaviour
{
    [SerializeField] private Transform view;

    private float yViewPosition;
    private bool  isShown;

    private void Start()
    {
        view.gameObject.SetActive(true);
        yViewPosition = view.position.y;
        view.Translate(0, Screen.height, 0);
    }
    
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
        if (newState is QuitState)
            Show();
        else
            Hide();
    }

    private void Show()
    {
        if (isShown) return;
        isShown = true;
        
        view.DOMoveY(yViewPosition, 0.6f);
    }

    private void Hide()
    {
        if (!isShown) return;
        isShown = false;

        view.DOMoveY(Screen.height + yViewPosition, 0.6f);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Stay()
    {
        GameState.Current = new FreeState();
    }
}