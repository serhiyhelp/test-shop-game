using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Character target;

    [Space]
    [SerializeField] private RectTransform view;
    [SerializeField] private Slot          hatSlot;
    [SerializeField] private Slot          robeSlot;
    [SerializeField] private RectTransform grid;
    [SerializeField] private Slot          cellPrefab;
    [SerializeField] private TMP_Text      moneyDisplay;

    private Slot[] _gridSlots;

    private float xViewPosition;

    private bool isShown;
    
    private void OnEnable()
    {
        GlobalEvents.GameStateChanged.AddListener(OnGameStateChanged);
    }


    private void OnDisable()
    {
        GlobalEvents.GameStateChanged.AddListener(OnGameStateChanged);
    }
    
    private void OnGameStateChanged(GameState newState)
    {
        if (newState is InventoryState)
            Show();
        else
            Hide();
    }

    private void Start()
    {
        view.gameObject.SetActive(true);
        xViewPosition = view.position.x;
        view.Translate(Screen.width/2, 0, 0);
        
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

    private void Show()
    {
        if (isShown) return;
        isShown = true;

        GlobalEvents.InterSlotExchange.AddListener(OnExchange);
        moneyDisplay.text = target.Money.ToString();
        hatSlot.AcceptItem(target.EquippedHat);
        robeSlot.AcceptItem(target.EquippedRobe);
        for (var i = 0; i < _gridSlots.Length; i++)
        {
            _gridSlots[i].AcceptItem(target.Inventory[i]);
        }

        view.DOMoveX(xViewPosition, 0.6f);
    }

    private void Hide()
    {
        if (!isShown) return;
        isShown = false;
        
        view.DOMoveX(Screen.width / 2 + xViewPosition, 0.6f);
        GlobalEvents.InterSlotExchange.RemoveListener(OnExchange);
    }

    private void OnExchange(Slot slot1, Slot slot2)
    {
        target.PutItemByInventoryId(slot1.Content?.Item, slot1.Id);
        target.PutItemByInventoryId(slot2.Content?.Item, slot2.Id);
    }
}