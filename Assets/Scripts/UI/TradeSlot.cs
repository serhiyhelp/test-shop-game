using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TradeSlot : MonoBehaviour
{
    [SerializeField] private Slot       owner;
    [SerializeField] private TMP_Text   priceTag;
    [SerializeField] private GameObject tradeIndicator;

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
        tradeIndicator.SetActive(false);
    }

    private void OnOwnerContentChanged(Item item, Slot _)
    {
        priceTag.text = item != null ? item.basePrice.ToString() : "";
        tradeIndicator.SetActive(item && owner.IsOwnedByMerchant != owner.Content.MerchantOrigin);
    }
}