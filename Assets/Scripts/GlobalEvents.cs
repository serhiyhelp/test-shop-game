using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvents
{
    public static UnityEvent<bool> TimeToOpenInventory = new UnityEvent<bool>();
}
