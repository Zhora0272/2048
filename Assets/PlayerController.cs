using System;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speed;
    private Vector3 _firstPosition;
    
    private void OnEnable()
    {
        _firstPosition = transform.position;
    }

    private void Update()
    {
        var mouseXPosition = _camera.ScreenToWorldPoint(Input.mousePosition).x;
        
        transform.position = Vector3.Lerp(transform.position, new Vector3(mouseXPosition, _firstPosition.y,_firstPosition.z), Time.deltaTime * _speed);
    }
}
