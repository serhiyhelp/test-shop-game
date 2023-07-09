using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Space]
    [SerializeField] private Item content;

    private void OnValidate()
    {
        if (content)
        {
            spriteRenderer.sprite = content.onGroundView;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<Character>(out var character) && character.TryPickUpItem(content))
        {
            Destroy(gameObject);
        }
    }
}