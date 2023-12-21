﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public abstract class EyeBaseController : CachedMonoBehaviour,
    IEyeParameters, IEyebattleParameters, IUpdateable<UpdateElementModel>
{
    //
    public IReactiveProperty<int> Mass => _hp;
    public IReactiveProperty<float> Speed => _speed;
    public float Force => Rb.mass * Rb.velocity.magnitude;
    public Vector3 Position => transform.position;
    public Transform EyeTransform => transform;

    public IReactiveProperty<int> KillCount => _killCount;
    //

    //readonly reactive properties
    private readonly ReactiveProperty<int> _hp = new(100);
    private readonly ReactiveProperty<int> _killCount = new(0);

    private readonly ReactiveProperty<float> _speed = new(25);
    //

    [Space] [SerializeField] protected BrokenEyePartsController _brokenEyePartsController;
    [SerializeField] protected BrokenEyeCollection _brokenEyeCollector;
    [SerializeField] protected Rigidbody Rb;
    [Space] [SerializeField] protected Material _material;
    [SerializeField] protected GameObject _meshRenderer;
    [SerializeField] protected Transform _eyeModelTransform;
    [SerializeField] protected SphereCollider _sphereCollider;
    [Space] [SerializeField] protected Image _loadbar;
    [SerializeField] protected TriggerCheckController _triggerCheckController;
    [Space] [SerializeField] private UpdateController _updateController;

    [field: SerializeField] public ReactiveProperty<float> Size { protected set; get; }
    [field: SerializeField] public ReactiveProperty<bool> IsDeath { private set; get; }

    protected Vector3 moveDirection;
    private Vector3 _lastPosition;

    #region UnityEvents

    private IDisposable _brokenEyeCollectorDisposable;
    private IDisposable _sizeDisposable;
    private IDisposable _everyUpdateDispose;

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        
        _currentModelClone = GetEyeConfigModel();
        _triggerCheckController.TriggerLayerEnterRegister(Layer.Eye, EyeAttackCheck);
        _updateController.UpdateElementController.Subscribe(GetUpdate).AddTo(this);

        IsDeath.Subscribe(state =>
        {
            if (state)
            {
                EyeDeadEvent();
            }
            
        }).AddTo(this);
    }

    protected virtual void Start()
    {
        //_brokenEyeCollector.BrokenPartsCollectionStream.Subscribe(value => { Size.Value += value; }).AddTo(this); //this part change the eye size after collection points ("broken eyes") 

        Size.Subscribe(value =>
        {
            _loadbar.DOFillAmount(((int)(value / 100) + 1) - ((float)value / 100), 1);

            if (value % 100 == 0)
            {
                transform.DOScale((value / 300) + 1, 1);

                _loadbar.DOKill();
                _loadbar.DOFillAmount(0, 1);
            }
        }).AddTo(this);
    }

    //Updateable part
    private EyeModelBase _currentModelClone;
    public void GetUpdate(UpdateElementModel model)
    {
        if(model == null) return;
        
        _currentModelClone = GetEyeConfigModel();

        AddUpdate(model);

        if (model.UpdateTime > 0)
        {
            Observable.Timer(TimeSpan.FromSeconds(model.UpdateTime)).Subscribe(_ =>
            {
                CancelUpdate();

                print("update time is loss");

            }).AddTo(this);
        }
        else
        {
            SetupNewModel(model);
        }
    }

    private void AddUpdate(UpdateElementModel model)
    {
        Speed.Value += model.Speed;
    }
    private void CancelUpdate()
    {
        Speed.Value = _currentModelClone.Speed;
    }
    private void SetupNewModel(UpdateElementModel model)
    {
        _currentModelClone.Speed = model.Speed;
    }
    private EyeModelBase GetEyeConfigModel()
    {
        return new EyeModelBase()
        {
            Speed = Speed.Value,
        };
    }
    // end Updateable part
    

    protected virtual void EyeDeadEvent()
    {
        _brokenEyeCollectorDisposable?.Dispose();
        _sizeDisposable?.Dispose();
        _everyUpdateDispose?.Dispose();

        _sphereCollider.enabled = false;
        _meshRenderer.SetActive(false);
        Rb.isKinematic = true;
    }

    protected void MoveBalanceStart()
    {
        MoveBalanceStop();
        _everyUpdateDispose = Observable.EveryUpdate().Subscribe(_ =>
        {
            Rb.velocity = Vector3.Lerp(Rb.velocity, Vector3.zero, Time.deltaTime);
            Rb.angularVelocity = Vector3.Lerp(Rb.angularVelocity, Vector3.zero, Time.deltaTime);

            EyeRotate();
        }).AddTo(this);
    }

    protected void MoveBalanceStop()
    {
        _everyUpdateDispose?.Dispose();
    }

    private void EyeRotate()
    {
        if (_lastPosition - transform.position != Vector3.zero && (_lastPosition - transform.position).magnitude < 1)
        {
            Quaternion rotation = Quaternion.LookRotation(transform.position - _lastPosition, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5);
            _eyeModelTransform.Rotate((Rb.velocity.magnitude * Time.deltaTime * 80), 0, 0);
        }
    }

    protected virtual void FixedUpdate()
    {
        _lastPosition = transform.position;

        Move();
    }

    #endregion

    protected abstract void Move();

    private void EyeAttackCheck(Collider other)
    {
        if (other.gameObject.TryGetComponent<EyeBaseController>(out var result))
        {
            result.Attack(Rb.mass * Rb.velocity.magnitude, transform.position);
        }
    }

    private void Attack(float force, Vector3 attackPosition)
    {
        if (Rb.mass * Rb.velocity.magnitude < force)
        {
            IsDeath.Value = true;
            _brokenEyePartsController.Activate(_material, attackPosition);
        }
    }
}