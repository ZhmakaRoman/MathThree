using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AnimationElementAppearance : MonoBehaviour
{
    private void Start()
    {
        SpawnAnimation();
    }
    
    private void SpawnAnimation()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(0.3f, 0.45f, 0.1f), 1);
    }

    public void AnimationDeleteElement()
    {
        transform.DOScale(new Vector3(0,0,0), 1);
    }

    public void MoveElement(Vector2 tempPosition)
    {
        transform.DOMove(tempPosition, 0.6f);
    }
    
    
}
