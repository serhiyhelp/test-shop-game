using ManaSeedTools.CharacterAnimator;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [SerializeField] private Item[] inventory = new Item[16];
    [SerializeField] private Item   equippedHat;
    [SerializeField] private Item   equippedRobe;
    [SerializeField] private Player _player;

    [SerializeField] private int money = 225;

    public UnityEvent<int> MoneyChanged;

    public Item[] Inventory
    {
        get => inventory;
        set => inventory = value;
    }

    public Item EquippedHat
    {
        get => equippedHat;
        set => equippedHat = value;
    }

    public Item EquippedRobe
    {
        get => equippedRobe;
        set => equippedRobe = value;
    }

    public int HatId
    {
        get => -1;
    }

    public int RobeId
    {
        get => -2;
    }

    public int Money
    {
        get => money;
        set
        {
            money = value;
            MoneyChanged?.Invoke(value);
        }
    }

    public void PutItemByInventoryId(Item item, int id)
    {
        if (id == HatId)
        {
            equippedHat = item;
            _player.ChangeHat(item?.equippedView);
        }
        else if (id == RobeId)
        {
            equippedRobe = item;
            _player.ChangeOutfit(item?.equippedView);
        }
        else
            inventory[id] = item;
    }

    private void Start()
    {
        _player.ChangeOutfit(equippedRobe?.equippedView);
        _player.ChangeHat(equippedHat?.equippedView);
    }

    public bool TryPickUpItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                return true;
            }
        }

        return false;
    }
}