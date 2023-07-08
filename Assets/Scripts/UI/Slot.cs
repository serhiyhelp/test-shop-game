using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public UnityEvent<Item, int> contentChanged;
    public RectTransform         rectTransform;
    public DndItem               dndPrefab;
    public ClothType             Filter;

    public DndItem Content
    {
        get;
        set;
    }

    public int Id
    {
        get;
        private set;
    }
    

    public void Init(int id, Item item, UnityAction<Item, int> contentChangedAction)
    {
        Id = id;
        if (item != null)
        {
            Content = Instantiate(dndPrefab, transform);
            Content.Init(this, item);
        }
        contentChanged.AddListener(contentChangedAction);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<DndItem>(out var dropped) && (Filter == ClothType.None || Filter == dropped.Item.type))
        {
            var oppositeSlot = dropped.Container;
            if (oppositeSlot == this) return;

            oppositeSlot.Content = this.Content;
            if (Content != null)
            {
                Content.Container = oppositeSlot;
                this.Content.MoveToContainer();
            }
            oppositeSlot.contentChanged.Invoke(this.Content?.Item, oppositeSlot.Id);

            this.Content      = dropped;
            dropped.Container = this;
            dropped.MoveToContainer();
            contentChanged.Invoke(Content.Item, Id);
        }
    }

    private void OnValidate()
    {
        if (!rectTransform)
        {
            rectTransform = GetComponent<RectTransform>();
        }
    }
}