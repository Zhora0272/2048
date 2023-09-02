using DG.Tweening;
using System;
using UniRx;
using UnityEngine;

public class BrokenEyeCollection : MonoBehaviour
{
    public IObservable<float> BrokenPartsCollectionStream => _breokenPartCollectionSubject;
    private Subject<float> _breokenPartCollectionSubject = new();

    private IDisposable _updateDisposable;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("BrokenEye"))
        {
            if (other.TryGetComponent<Collectable>(out var result))
            {
                other.transform.DOMove(other.transform.position + Vector3.up, 0.5f);

                var eyeTransform = other.transform;

                var value = result.Collect();

                _breokenPartCollectionSubject.OnNext(value);

                _updateDisposable = Observable.EveryUpdate().Subscribe(_ =>
                {
                    eyeTransform.position = Vector3.Lerp(eyeTransform.position,
                        transform.position,
                        Time.deltaTime * 3);

                }).AddTo(this);

            }
        }
    }

    private void OnDisable()
    {
        _updateDisposable?.Dispose();
    }
}