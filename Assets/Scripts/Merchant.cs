using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] private float  _additionalPrice = 1.2f;
    [SerializeField] private Item[] possibleItems;
    [SerializeField] private int    maxItems         = 4;
    [SerializeField] private float  itemsChangeTimer = 5f;

    private float lastInteractionTime = -1000;

    public Item[] Inventory { get; set; } = new Item[45];
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        GlobalEvents.Engagement.AddListener(OnEngagement);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        GlobalEvents.Engagement.RemoveListener(OnEngagement);
    }

    public void UpdateInteractionTime()
    {
        lastInteractionTime = Time.time;
    }

    private void OnEngagement()
    {
        if (Time.time - lastInteractionTime > itemsChangeTimer)
        {
            var rm = new System.Random();
            Inventory = new Item[45];
            for (int i = 0; i < maxItems; i++)
            {
                Inventory[i] = possibleItems[rm.Next(0, possibleItems.Length)];
            }
        }
        UpdateInteractionTime();
        GlobalEvents.Trade.Invoke(this);
    }

}