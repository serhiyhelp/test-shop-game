using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriceTag : MonoBehaviour
{
    [SerializeField] private Slot     owner;
    [SerializeField] private TMP_Text label;

    private void OnEnable()
    {
        owner.contentChanged.AddListener(OnOwnerContentChanged);
    }
    private void OnDisable()
    {
        owner.contentChanged.RemoveListener(OnOwnerContentChanged);
    }

    private void Start()
    {
        OnOwnerContentChanged(owner.Content?.Item, null);
    }

    private void OnOwnerContentChanged(Item item, Slot _)
    {
        label.text = item != null ? item.basePrice.ToString() : "";
    }
}