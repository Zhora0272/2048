using System;
using UnityEngine;

public class EyeCustomizeController : MonoBehaviour, ISaveable
{
    [SerializeField] private Material _eyeMaterial;
    [SerializeField] private MeshRenderer _eyeMeshRenderer;
    [SerializeField] private GameObject _decorGamobject;

    private EyeCustomizeModel _model;

    public Material GetMaterial() => _eyeMaterial;
    public GameObject GetDecor() => _decorGamobject;

    private void Awake()
    {
        _model = new EyeCustomizeModel();
    }

    public void SetData(GameData data)
    {
        _eyeMaterial = EyeShaderGraph.GetMaterial(data.EyeConfigModel);
        _eyeMeshRenderer.material = _eyeMaterial;
    }

    public GameData GetData()
    {
        return new GameData()
        {
            EyeConfigModel = _model,
        };
    }
}