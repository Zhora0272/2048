using System;
using System.Collections.Generic;
using UnityEngine;

public class GunAmmoController : MonoBehaviour
{
    [Serializable]
    private class AmmoMagazine
    {
        public GunAmmoBase Ammo;
        public Transform AmmoContentPosition;
        public bool AmmoExistState;
    }

    [SerializeField] private List<AmmoMagazine> _ammoSpawnTransforms;
    [SerializeField] private int _ammoCount;
    [SerializeField] private int _ammoCapacity;
    [SerializeField] private GunAmmoBase _ammoPrefab;

    private AmmoPool _ammoPool;
    private int _spawnCount;

    private void Awake()
    {
        _spawnCount = _ammoSpawnTransforms.Count;
        _ammoPool = new AmmoPool();

        Init();
    }
    
    private void Init()
    {
        if (_spawnCount > 1)
        {
            _reloadAction = () =>
            {
                foreach (var item in _ammoSpawnTransforms)
                {
                    if (item.AmmoExistState) continue;

                    Reload(item);
                }
            };
        }
        else
        {
            _reloadAction = () => { Reload(_ammoSpawnTransforms[0]); };
        }
    }

    private Action _reloadAction;

    private void OnEnable()
    {
        TryReloadAmmo();
    }

    public void TryReloadAmmo()
    {
        _reloadAction.Invoke();
    }

    private void Reload(AmmoMagazine item)
    {
        _ammoCount--;

        var ammo = _ammoPool.GetPoolElement(AmmoType.Rocket, _ammoPrefab, item.AmmoContentPosition);
        item.Ammo = ammo;
        item.AmmoExistState = true;

        Transform ammoTransform = ammo.transform;

        ammoTransform.localPosition = Vector3.zero; //
        ammoTransform.localRotation = Quaternion.identity;
    }

    //reload before get ammo
    public bool GetAmmo(out GunAmmoBase ammo)
    {
        foreach (var item in _ammoSpawnTransforms)
        {
            if (item.AmmoExistState)
            {
                ammo = item.Ammo;
                item.AmmoExistState = false;
                return true;
            }
        }

        ammo = null;
        return false;
    }
}