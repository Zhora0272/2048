using System.Globalization;
using Shop.Container;
using System;
using Saveing;
using UnityEngine;

public class SaveSystem : MonoManager
{
    [SerializeField] private EyeCustomizeController _eyeCustomizeController;
    [SerializeField] private ShopContainerManager _shopContainerManager;

    private JsonHelper _dataSave;

    #region ISaveable

    private IGameDataSaveable _financeManagerGameDataSaveable;
    private IGameDataSaveable _eyeCustomizeGameDataSaveable;
    private IGameDataSaveable _shopContainerGameDataSaveable;

    #endregion

    //
    private GameData _gameData;

    //
    private DataManager _dataManager;

    protected override void Awake()
    {
        base.Awake();
        _dataSave = new JsonHelper();
    }

    private void Start()
    {
        Init();
        InitData();
        SetData();
    }

    private void Init()
    {
        _financeManagerGameDataSaveable = MainManager.GetManager<FinanceManager>();
        _dataManager = MainManager.GetManager<DataManager>();

        _eyeCustomizeGameDataSaveable = _eyeCustomizeController;
        _shopContainerGameDataSaveable = _shopContainerManager;
    }

    private void InitData()
    {
        #region Set Deafult Variables For First

        //set default variables
        if (!_dataSave.ExistData())
        {
            var data = new GameData
            {
                Money = 10000,
                Gem = 15,
                ContainerConfigIndexes = new[] {0, 0, 0, 0},
                EyeItemParameters = _dataManager.GetAllDataLists(),
                EyeConfigModel = new EyeCustomizeModel
                {
                    _eyeSize = 3.37f,
                    _eyeBibeSize = 2.24f,
                    _eyeColor = 1,
                    _eyeBackColor = 2
                }
            };
            _dataSave.SaveData(data);
        }

        #endregion

        _gameData = _dataSave.GetData();
    }

    private void SetData()
    {
        var financeData = _financeManagerGameDataSaveable.GetData();
        var playerEyeData = _eyeCustomizeGameDataSaveable.GetData();
        var containerManager = _shopContainerGameDataSaveable.GetData();

        financeData.Money = _gameData.Money;
        financeData.Gem = _gameData.Gem;

        playerEyeData.EyeConfigModel = _gameData.EyeConfigModel;

        containerManager.EyeItemParameters = _gameData.EyeItemParameters;
        containerManager.ContainerConfigIndexes = _gameData.ContainerConfigIndexes;

        _financeManagerGameDataSaveable.SetData(financeData);
        _eyeCustomizeGameDataSaveable.SetData(playerEyeData);
        _shopContainerGameDataSaveable.SetData(containerManager);
    }

    public void SaveData()
    {
        var financeData = _financeManagerGameDataSaveable.GetData();
//        var playerEyeData = _eyeCustomizeGameDataSaveable.GetData();
        var shopContainerData = _shopContainerGameDataSaveable.GetData();

        _gameData.Gem = financeData.Gem;
        _gameData.Money = financeData.Money;
        _gameData.EyeConfigModel = _eyeCustomizeGameDataSaveable.GetData().EyeConfigModel;

        _gameData.EyeItemParameters = shopContainerData.EyeItemParameters;
        _gameData.ContainerConfigIndexes = shopContainerData.ContainerConfigIndexes;

        _gameData.SaveTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        
        _dataSave.SaveData(_gameData);
    }
}