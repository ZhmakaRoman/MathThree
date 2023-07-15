using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemType Type { get; private set; }

    public bool IsMatched { get; set; } // Временное поле

    public void Show(float tweenDuration)
    {
        // Анимация увеличения размера элемента
        var itemScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(itemScale, tweenDuration);
    }
    
    public Tween Move(Vector2Int position, float tweenDuration)
    {
        // Анимация перемещения элемента
        var tween = transform.DOMove(new Vector3(position.x, position.y), tweenDuration);
        return tween;
    }
  
}
