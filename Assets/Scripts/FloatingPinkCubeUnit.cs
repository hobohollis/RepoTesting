using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FloatingPinkCubeUnit : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        HandleSpawnAnimation();
    }
    
    
    private IEnumerator OnSpawnMovement()
    {
        transform.DOScale(.5f, .01f);
        transform.DOMove(new Vector3(this.transform.position.x,transform.position.y +3.5f, transform.position.z), 3f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(1f);
        transform.DOScale(1f, 2f);
    }

    public override void HandleSpawnAnimation()
    {
        StartCoroutine(OnSpawnMovement());
    }
}