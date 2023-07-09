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
    [SerializeField] private GameObject priceTagBg;
    [SerializeField] private GameObject tradeIndicator;

    private void OnEnable()
    {
        owner.contentChanged.AddListener(OnOwnerContentChanged);
        OnOwnerContentChanged(owner.Content?.Item, null);
    }
    private void OnDisable()
    {
        owner.contentChanged.RemoveListener(OnOwnerContentChanged);
    }

    private void OnOwnerContentChanged(Item item, Slot _)
    {
        if (item)
        {
            priceTagBg.SetActive(true);
            priceTag.text = item.basePrice.ToString();
            tradeIndicator.SetActive(owner.IsOwnedByMerchant != owner.Content.MerchantOrigin);
        }
        else
        {
            priceTagBg.SetActive(false);
            tradeIndicator.SetActive(false);
        }
    }
}