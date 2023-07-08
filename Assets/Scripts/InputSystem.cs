using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GlobalEvents.TimeToOpenInventory.Invoke(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalEvents.TimeToOpenInventory.Invoke(false);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            GlobalEvents.Engagement.Invoke();
        }
        
    }
}