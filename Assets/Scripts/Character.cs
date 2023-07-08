using ManaSeedTools.CharacterAnimator;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Item[] inventory = new Item[16];
    [SerializeField] private Item   equippedHat;
    [SerializeField] private Item   equippedRobe;
    [SerializeField] private Player _player;

    [SerializeField] private int money = 225;
    

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
        set => money = value;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void SwapItems(int id1, int id2)
    {
        var item1 = GetItemByInventoryId(id1);
        var item2 = GetItemByInventoryId(id2);

        PutItemByInventoryId(item1, id2);
        PutItemByInventoryId(item2, id1);
    }

    private Item GetItemByInventoryId(int id)
    {
        if (id == HatId)
            return equippedHat;
        if (id == RobeId)
            return equippedRobe;
        return inventory[id];
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

    public void EquipHat(Item hat, int slotId)
    {
        
    }

    public void EquipRobe(Item robe)
    {
        
    }

    private void OnHatChanged()
    {
        
    }
}