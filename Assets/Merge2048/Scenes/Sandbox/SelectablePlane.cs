using UnityEngine;
using System;
using System.Linq;
using TMPro;
using UniRx;
using Random = UnityEngine.Random;

public class SelectablePlane : MonoBehaviour, ISelectableManager
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _mergeCountText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    
    private SelectableGrid[] _selectableGrid;
    [field:SerializeField] public Data2048 Data { get; private set; }
    public Action MergeCallback { get; private set; }
    
    private int _score;
    private int _mergeCount;
    private int _bestScore;

    private enum PlayerPrefsEnum
    {
        WeaponMergeSaveIndex,
        InitialStart,
        BestScore,
        Score
    }

    private void Awake()
    {
        _selectableGrid = GetComponentsInChildren<SelectableGrid>();
    }

    private bool _skipSpawn;
    private void Start()
    { 
        var initialState = PlayerPrefs.GetInt(PlayerPrefsEnum.InitialStart.ToString());
        
        for (int i = 0; i < _selectableGrid.Length; i++)
        {
            _selectableGrid[i].SetManager(this, Data);
        }

        PlayerPrefs.SetInt(PlayerPrefsEnum.InitialStart.ToString(), 1);
        
        if (initialState == 1)
        {
            for (int i = 0; i < _selectableGrid.Length; i++)
            {
                var index = PlayerPrefs.GetInt(PlayerPrefsEnum.WeaponMergeSaveIndex.ToString() + i, -1);
                if (index != -1)
                {
                    _selectableGrid[i].SetObject(index);

                    _score += index;
                }
            }

            _scoreText.text = _score.ToString();
        }
        else
        {
            _selectableGrid[0].SetObject();
            _selectableGrid[Random.Range(1, _selectableGrid.Length - 1)].SetObject();   
        }

        Observable.Interval(TimeSpan.FromSeconds(Random.Range(3f,5f))).Subscribe(_ =>
        {
            if (_skipSpawn)
            {
                _skipSpawn = false;
                return;
            }

            RandomSpawn();

        }).AddTo(this);

        
        MergeCallback = () =>
        {
            Observable.Timer(TimeSpan.FromSeconds(Random.Range(0.2f, 0.8f))).Subscribe(_ =>
            {
                RandomSpawn();

            }).AddTo(this);
        };

        /*var mergeCount = PlayerPrefs.GetInt(PlayerPrefsEnum.WeaponMergeBuyCount.ToString(), 0);
        
        if (mergeCount == 0)
        {
            bool state = false;
            
            for (int i = 0; i < _selectableGrid.Length; i++)
            {
                if (_selectableGrid[i].GetObject(out var rs, false))
                {
                    state = true;
                }
            }
            if (!state)
            {
                _selectableGrid[0].SetObject();
            }
        }*/
    }

    private float _lastSpawnTime;
    private void RandomSpawn()
    {
        if (Time.time - _lastSpawnTime < 1) return;
        _lastSpawnTime = Time.time;

        var freeGrides = from item in _selectableGrid where (!item.CheckObject()) select (item);
        var count = freeGrides.Count();
        var randomIndex = Random.Range(0, count - 1);

        if (freeGrides.Count() > 0)
        {
            freeGrides.ToArray()[randomIndex].SetObject();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _selectableGrid.Length; i++)
        {
            PlayerPrefs.SetInt(PlayerPrefsEnum.WeaponMergeSaveIndex.ToString() + i,
                _selectableGrid[i].GetUpgradeIndex());
        }
    }
}

public interface ISelectableManager
{
    public Data2048 Data { get; }
    public Action MergeCallback { get; }  
}