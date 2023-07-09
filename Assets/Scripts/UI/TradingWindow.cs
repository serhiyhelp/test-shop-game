using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradingWindow : MonoBehaviour
{
    [SerializeField] private Character target;
    [Space]
    [SerializeField] private RectTransform leftView;
    [SerializeField] private RectTransform rightView;
    [SerializeField] private GameObject    staticView;
    [Space]
    [SerializeField] private Transform merchantGrid;
    [SerializeField] private Transform playerGrid;
    [SerializeField] private Slot      cellPrefab;
    [SerializeField] private TMP_Text  acceptButtonLabel;
    [SerializeField] private Button    acceptButton;

    [SerializeField] private int inventoryLength = 45;
    
    private Slot[] _playerSlots;
    private Slot[] _merchantSlots;

    private Item[] _merchantOriginalInventory;
    private Item[] _playerOriginalInventory;

    private Merchant _merchant;
    private string   acceptButtonFormat;
    private bool     isShown;

    private int   totalPrice;
    private float xLeftPosition;
    private float xRightPosition;
    
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
        if (newState is TradeState tradeState)
            Show(tradeState.Merchant);
        else if (isShown)
            Decline();
    }

    private void Awake()
    {
        acceptButtonFormat = acceptButtonLabel.text;
    }

    private void Start()
    {
        staticView.SetActive(false);
        xLeftPosition  = leftView.position.x;
        xRightPosition = rightView.position.x;
        leftView.Translate(-Screen.width/2, 0, 0);
        rightView.Translate(Screen.width/2, 0, 0);
        
        _merchantSlots = new Slot[inventoryLength];
        for (var i = 0; i < inventoryLength; i++)
        {
            var cell = Instantiate(cellPrefab, merchantGrid);
            cell.Init(i, true);
            _merchantSlots[i] = cell;

        }

        _playerSlots = new Slot[target.Inventory.Length];
        for (var i = 0; i < target.Inventory.Length; i++)
        {
            var cell = Instantiate(cellPrefab, playerGrid);
            cell.Init(i, false);
            _playerSlots[i] = cell;
        }
    }

    public void Show(Merchant merchant)
    {
        if (isShown) return;
        isShown = true;

        acceptButtonLabel.text     = string.Format(acceptButtonFormat, 0);
        totalPrice                 = 0;
        _merchant                  = merchant;
        _merchantOriginalInventory = merchant.Inventory.ToArray();
        _playerOriginalInventory   = target.Inventory.ToArray();
        for (var i = 0; i < _playerSlots.Length; i++)
        {
            _playerSlots[i].SetItem(target.Inventory[i]);
        }
        
        for (var i = 0; i < _merchantSlots.Length; i++)
        {
            _merchantSlots[i].SetItem(merchant.Inventory[i]);
        }
        
        GlobalEvents.InterSlotExchange.AddListener(OnExchange);
        
        UpdateAcceptButton();
        
        staticView.SetActive(true);
        rightView.DOMoveX(xRightPosition, 0.6f);
        leftView.DOMoveX(xLeftPosition, 0.6f);
    }

    public void Decline()
    {
        _merchant.Inventory = _merchantOriginalInventory;
        target.Inventory    = _playerOriginalInventory;
        Hide();
        _merchant.UpdateInteractionTime();
        GlobalEvents.InterSlotExchange.RemoveListener(OnExchange);
    }

    public void TryAccept()
    {
        if (target.Money >= totalPrice)
        {
            target.Money -= totalPrice;
            Hide();
            _merchant.UpdateInteractionTime();
            GlobalEvents.InterSlotExchange.RemoveListener(OnExchange);
            GameState.Current = new FreeState();
        }
    }

    private void UpdateAcceptButton()
    {
        acceptButton.interactable = target.Money >= totalPrice;
    }

    private void Hide()
    {
        if (!isShown) return;
        isShown = false;
        
        staticView.SetActive(false);
        rightView.DOMoveX(xRightPosition + Screen.width/2, 0.6f);
        leftView.DOMoveX(xLeftPosition - Screen.width/2, 0.6f);
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
            UpdateAcceptButton();
        }
    }
}