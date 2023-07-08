using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] private float _additionalPrice = 1.2f;

    public Item[] Inventory = new Item[15];
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        GlobalEvents.Engagement.AddListener(OnEngagement);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        GlobalEvents.Engagement.RemoveListener(OnEngagement);
    }

    private void OnEngagement()
    {
        GlobalEvents.Trade.Invoke(this);
    }

}