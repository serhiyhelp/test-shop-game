using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject placeholder;
    
    public UnityEvent<Item, Slot> contentChanged;
    public RectTransform          rectTransform;
    public DndItem                dndPrefab;
    public ClothType              Filter;

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
    

    public void Init(int id, Item item)
    {
        Id = id;
        if (item != null)
        {
            Content = Instantiate(dndPrefab, transform);
            Content.Init(this, item);
            if (placeholder) placeholder.SetActive(false);
        }
        //contentChanged.AddListener(contentChangedAction);
    }

    public void AcceptItem(Item item)
    {
        if (item)
        {
            if (Content == null)
            {
                Content = Instantiate(dndPrefab, transform);
                Content.Init(this, item);
                if (placeholder) placeholder.SetActive(false);
            }
            else
            {
                Content.Item = item;
            }
        }
        else
        {
            if (Content)
            {
                Destroy(Content.gameObject);
                Content = null;
                if (placeholder) placeholder.SetActive(true);
            }
        }
    }

    private void ContentChangedNotify()
    {
        contentChanged.Invoke(Content?.Item, this);
        if (placeholder) placeholder.SetActive(!Content);
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
            
            //oppositeSlot.contentChanged.Invoke(this.Content?.Item, oppositeSlot);
            oppositeSlot.ContentChangedNotify();

            this.Content      = dropped;
            dropped.Container = this;
            dropped.MoveToContainer();
            //contentChanged.Invoke(Content.Item, this);
            ContentChangedNotify();
            
            GlobalEvents.InterSlotExchange.Invoke(this, oppositeSlot);
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