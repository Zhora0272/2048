using System;
using UniRx;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Ball")) return;
        
        Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ =>
        {
            gameObject.SetActive(false);
        }).AddTo(this);
    }
}
