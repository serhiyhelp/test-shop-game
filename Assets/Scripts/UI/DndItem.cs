using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DndItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image         _image;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float         tooltipDelay = 0.5f;

    private Item _item;

    private bool    trackMouse;
    private float   lastTimeMouseMoved;
    private Vector3 lastMousePos;

    public bool MerchantOrigin
    {
        get;
        set;
    }

    public Item Item
    {
        get => _item;
        set
        {
            _item         = value;
            _image.sprite = Item.uiView;
        }
    }

    public Slot Container
    {
        get;
        set;
    }

    public void Init(Slot container, Item item, bool merchantOrigin = false)
    {
        Item           = item;
        Container      = container;
        MerchantOrigin = merchantOrigin;
    }

    public void Update()
    {
        if (!trackMouse) return;

        if ((Input.mousePosition - lastMousePos).sqrMagnitude > 1 )
        {
            lastTimeMouseMoved = Time.time;
            if (ToolTip.Instance.IsShown)
            {
                ToolTip.Instance.Hide();
            }
        }

        if (!ToolTip.Instance.IsShown && Time.time - lastTimeMouseMoved > tooltipDelay)
        {
            ToolTip.Instance.Show(_item);
        }
        
        lastMousePos = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Container = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(Container.RectTransform);
        _rectTransform.anchoredPosition = Vector2.zero;
        _image.raycastTarget            = true;
    }

    public void MoveToContainer()
    {
        transform.SetParent(Container.RectTransform);
        _rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        trackMouse = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        trackMouse = false;
    }
    
    
}