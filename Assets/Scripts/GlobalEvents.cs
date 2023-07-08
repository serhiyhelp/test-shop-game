using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvents
{
    public static UnityEvent<bool>                   TimeToOpenInventory = new();
    public static UnityEvent                         Engagement          = new();
    public static UnityEvent<Merchant>               Trade               = new();
    public static UnityEvent<Slot, Slot> InterSlotExchange   = new();
}