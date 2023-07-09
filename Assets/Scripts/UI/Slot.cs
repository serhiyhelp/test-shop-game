using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform rectTransform;

    [SerializeField] private DndItem   dndPrefab;
    [SerializeField] private ClothType filter;
    
    public UnityEvent<Item> contentChanged;

    private DndItem _content;

    public DndItem Content
    {
        get => _content;
        set
        {
            _content = value;
            contentChanged?.Invoke(value?.Item);
        }
    }

    public int Id
    {
        get;
        private set;
    }

    public bool IsOwnedByMerchant
    {
        get;
        private set;
    }

    public RectTransform RectTransform
    {
        get => rectTransform;
        set => rectTransform = value;
    }


    public void Init(int id, bool isOwnedByMerchant = false)
    {
        Id                = id;
        IsOwnedByMerchant = isOwnedByMerchant;
    }

    public void SetItem(Item item)
    {
        if (item)
        {
            if (Content == null)
            {
                var dndItem = Instantiate(dndPrefab, rectTransform);
                dndItem.Init(this, item, IsOwnedByMerchant);
                Content = dndItem;
            }
            else
            {
                Content.MerchantOrigin = IsOwnedByMerchant;
                Content.Item           = item;
                contentChanged.Invoke(item);
            }
        }
        else
        {
            if (Content)
            {
                Destroy(Content.gameObject);
                Content = null;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<DndItem>(out var dropped) && (filter == ClothType.None || filter == dropped.Item.type))
        {
            var oppositeSlot = dropped.Container;
            if (oppositeSlot == this) return;

            oppositeSlot.Content = this.Content;
            if (Content != null)
            {
                Content.Container = oppositeSlot;
                this.Content.MoveToContainer();
            }
            
            this.Content      = dropped;
            dropped.Container = this;
            dropped.MoveToContainer();
            
            GlobalEvents.InterSlotExchange.Invoke(this, oppositeSlot);
        }
    }

    private void OnValidate()
    {
        if (!RectTransform)
        {
            RectTransform = GetComponent<RectTransform>();
        }
    }
}