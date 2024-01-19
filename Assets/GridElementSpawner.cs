using System.Collections.Generic;
using UnityEngine;

public class GridElementSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnObject;
    [SerializeField] private Transform _contextMenu;
    
    [SerializeField] private Vector2 _spawnGridCount;
    [SerializeField] private Vector2 _spawnGridOffset;

    private List<GameObject> _spawnObjList = new();

    [ContextMenu("spawn")]
    private void SpawnObj()
    {
        if (_spawnObjList != null && _spawnObjList.Count > 0)
        {
            for (int i = 0; i < _spawnObjList.Count; i++)
            {
                DestroyImmediate(_spawnObjList[i]);
            }

            _spawnObjList.Clear();
        }

        _spawnObjList ??= new List<GameObject>();
        
        for (int i = 0; i < _spawnGridCount.x; i++)
        {
            for (int j = 0; j < _spawnGridCount.y; j++)
            {
                var item = Instantiate(_spawnObject, _contextMenu);
                item.transform.localPosition = new Vector3(i + _spawnGridOffset.x, 0, j + _spawnGridOffset.y);
                _spawnObjList.Add(item);
            }
        }
    }
}