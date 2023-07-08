using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Character target;
    [Space]
    [SerializeField] private GameObject _view;
    [SerializeField] private Slot      hatSlot;
    [SerializeField] private Slot      robeSlot;
    [SerializeField] private Transform grid;
    [SerializeField] private Slot      cellPrefab;

    [Space]
    [SerializeField] private int cellsPerRow = 4;
    [SerializeField] private float spacing = 50f;
    
    private void OnEnable()
    {
        GlobalEvents.TimeToOpenInventory.AddListener(ChangeViewVisibility);
    }
    private void OnDisable()
    {
        GlobalEvents.TimeToOpenInventory.RemoveListener(ChangeViewVisibility);
    }

    private void Start()
    {
        hatSlot.Init(target.HatId, target.EquippedHat, target.PutItemByInventoryId);
        robeSlot.Init(target.RobeId, target.EquippedRobe, target.PutItemByInventoryId);
        
        int column = 0, row = 0;
        for (var i = 0; i < target.Inventory.Length; i++)
        {
            var cell = Instantiate(cellPrefab, grid);
            cell.rectTransform.anchoredPosition = new Vector2(column * spacing, -row * spacing);
            cell.Init(i, target.Inventory[i], target.PutItemByInventoryId);

            column++;
            if (column >= cellsPerRow)
            {
                column = 0;
                row++;
            }
        }
    }

    private void ChangeViewVisibility(bool show)
    {
        _view.SetActive(show);
    }
}