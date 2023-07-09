using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [SerializeField] private RectTransform tooltip;
    [SerializeField] private TMP_Text      title;
    [SerializeField] private TMP_Text      price;
    [SerializeField] private TMP_Text      description;

    private Vector3 lastMousePos;

    public static ToolTip Instance
    {
        get;
        private set;
    }

    public bool IsShown
    {
        get => tooltip.gameObject.activeSelf;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Hide();
    }
    
    public void Update()
    {
        if (IsShown && (Input.mousePosition - lastMousePos).sqrMagnitude > 1 )
        {
            Hide();
        }
        
        lastMousePos = Input.mousePosition;
    }

    public void Show(Item itemToShow)
    {
        tooltip.gameObject.SetActive(true);
        
        var mPos   = Input.mousePosition;
        var width2 = tooltip.sizeDelta.x * 0.5f;

        if (mPos.x < width2) mPos.x                = width2;
        if (mPos.x > Screen.width - width2) mPos.x = Screen.width - width2;

        mPos.y -= 10;

        tooltip.transform.position = mPos;

        title.text       = itemToShow.title;
        description.text = itemToShow.description;
        price.text       = itemToShow.basePrice.ToString();

    }

    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
    }
}