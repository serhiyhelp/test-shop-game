using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TradingWindow : MonoBehaviour
{
    [SerializeField] private Character  target;
    [SerializeField] private GameObject _view;
    [SerializeField] private int        cellsPerRow = 5;
    [SerializeField] private float      spacing     = 50f;
    [SerializeField] private Transform  merchantGrid;
    [SerializeField] private Transform  playerGrid;
    [SerializeField] private Slot       cellPrefab;
    [SerializeField] private TMP_Text   moneyDisplay;
    [SerializeField] private TMP_Text   acceptButtonLabel;

    [SerializeField] private int inventoryLength = 15;
    
    private Slot[] _playerSlots;
    private Slot[] _merchantSlots;

    private Item[] _merchantOriginalInventory;
    private Item[] _playerOriginalInventory;

    private Merchant _merchant;
    private string   acceptButtonFormat;

    private int totalPrice;
    
    
    private void OnEnable()
    {
        GlobalEvents.Trade.AddListener(Show);
    }

    private void OnDisable()
    {
        GlobalEvents.Trade.RemoveListener(Show);
    }

    private void Awake()
    {
        acceptButtonFormat = acceptButtonLabel.text;
    }

    private void Start()
    {
        int column = 0, row = 0;
        _merchantSlots = new Slot[inventoryLength];
        for (var i = 0; i < inventoryLength; i++)
        {
            var cell = Instantiate(cellPrefab, merchantGrid);
            cell.rectTransform.anchoredPosition = new Vector2(column * spacing, -row * spacing);
            cell.Init(i, null);
            _merchantSlots[i] = cell;

            column++;
            if (column >= cellsPerRow)
            {
                column = 0;
                row++;
            }
        }

        column       = row = 0;
        _playerSlots = new Slot[target.Inventory.Length];
        for (var i = 0; i < target.Inventory.Length; i++)
        {
            var cell = Instantiate(cellPrefab, playerGrid);
            cell.rectTransform.anchoredPosition = new Vector2(column * spacing, -row * spacing);
            cell.Init(i, target.Inventory[i]);
            _playerSlots[i] = cell;

            column++;
            if (column >= cellsPerRow)
            {
                column = 0;
                row++;
            }
        }
    }

    public void Show(Merchant merchant)
    {
        _view.SetActive(true);
        acceptButtonLabel.text     = string.Format(acceptButtonFormat, 0);
        totalPrice                 = 0;
        _merchant                  = merchant;
        moneyDisplay.text          = target.Money.ToString();
        _merchantOriginalInventory = merchant.Inventory.ToArray();
        _playerOriginalInventory   = target.Inventory.ToArray();
        for (var i = 0; i < _playerSlots.Length; i++)
        {
            _playerSlots[i].AcceptItem(target.Inventory[i]);
        }
        
        for (var i = 0; i < _merchantSlots.Length; i++)
        {
            _merchantSlots[i].AcceptItem(merchant.Inventory[i]);
        }
        
        GlobalEvents.InterSlotExchange.AddListener(OnExchange);
    }

    public void Decline()
    {
        _merchant.Inventory = _merchantOriginalInventory;
        target.Inventory    = _playerOriginalInventory;
        _view.SetActive(false);
        GlobalEvents.InterSlotExchange.RemoveListener(OnExchange);
    }

    public void TryAccept()
    {
        if (target.Money >= totalPrice)
        {
            target.Money -= totalPrice;
            
            _view.SetActive(false);
            GlobalEvents.InterSlotExchange.RemoveListener(OnExchange);
        }
    }

    private void OnExchange(Slot slot1, Slot slot2)
    {
        var slot1OwnerIsPlayer = _playerSlots.Contains(slot1);
        var slot2OwnerIsPlayer = _playerSlots.Contains(slot2);

        if (slot1OwnerIsPlayer && slot2OwnerIsPlayer)
        {
            target.PutItemByInventoryId(slot1.Content?.Item, slot1.Id);
            target.PutItemByInventoryId(slot2.Content?.Item, slot2.Id);
        }
        else if (!slot1OwnerIsPlayer && !slot2OwnerIsPlayer)
        {
            _merchant.Inventory[slot1.Id] = slot1.Content?.Item;
            _merchant.Inventory[slot2.Id] = slot2.Content?.Item;
        }
        else
        {
            var merchantSlot = slot1OwnerIsPlayer ? slot2 : slot1;
            var playerSlot   = slot1OwnerIsPlayer ? slot1 : slot2;

            target.PutItemByInventoryId(playerSlot.Content?.Item, playerSlot.Id);
            _merchant.Inventory[merchantSlot.Id] = merchantSlot.Content?.Item;

            totalPrice             += playerSlot.Content?.Item.basePrice ?? 0;
            totalPrice             -= merchantSlot.Content?.Item.basePrice ?? 0;
            acceptButtonLabel.text =  string.Format(acceptButtonFormat, -totalPrice);
        }
    }
}