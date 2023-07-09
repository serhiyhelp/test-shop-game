using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Character target;
    [Space]
    [SerializeField] private GameObject _view;
    [SerializeField] private Slot          hatSlot;
    [SerializeField] private Slot          robeSlot;
    [SerializeField] private RectTransform grid;
    [SerializeField] private Slot          cellPrefab;
    [SerializeField] private TMP_Text      moneyDisplay;

    private Slot[] _gridSlots;
    
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
        hatSlot.Init(target.HatId, target.EquippedHat);
        robeSlot.Init(target.RobeId, target.EquippedRobe);
        
        _gridSlots = new Slot[target.Inventory.Length];
        Slot cell = null;
        for (var i = 0; i < target.Inventory.Length; i++)
        {
            cell = Instantiate(cellPrefab, grid);
            cell.Init(i, target.Inventory[i]);
            _gridSlots[i] = cell;
        }
    }

    private void OnNewItemPutInSlot(Item item, Slot slot)
    {
        target.PutItemByInventoryId(item, slot.Id);
    }

    private void ChangeViewVisibility(bool show)
    {
        _view.SetActive(show);
        if (show)
        {
            GlobalEvents.InterSlotExchange.AddListener(OnExchange);
            moneyDisplay.text = target.Money.ToString();
            hatSlot.AcceptItem(target.EquippedHat);
            robeSlot.AcceptItem(target.EquippedRobe);
            for (var i = 0; i < _gridSlots.Length; i++)
            {
                _gridSlots[i].AcceptItem(target.Inventory[i]);
            }
            
        }
        else
        {
            GlobalEvents.InterSlotExchange.RemoveListener(OnExchange);
        }
    }

    private void OnExchange(Slot slot1, Slot slot2)
    {
        target.PutItemByInventoryId(slot1.Content?.Item, slot1.Id);
        target.PutItemByInventoryId(slot2.Content?.Item, slot2.Id);
    }
}