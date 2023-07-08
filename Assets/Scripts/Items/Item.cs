using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Item", fileName = "Item", order = 0)]
public class Item : ScriptableObject
{
    public string title;
    [Multiline(4)]
    public string description;
    [Space]
    public Sprite uiView;
    public Texture2D equippedView;
    public Sprite    onGroundView;
    [Space]
    public int basePrice;
    public ClothType type;

    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            title = name;
        }
    }
}

public enum ClothType
{
    None, Hat, Robe
}