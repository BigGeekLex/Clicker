using System;
using CoreSystems;
using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;

public class GameInit : MonoBehaviour
{
    [SerializeField] 
    private StaticData staticData;
    [SerializeField]
    private BusinessConfigSetup businessConfigSetup;
    [SerializeField] 
    private SceneData sceneData;
    
    private EcsWorld _world;
    private EcsSystems _gameplaySystems;
    private EcsSystems _saveSystems;

    private void Awake()
    {
        _world = new EcsWorld ();
        _gameplaySystems = new EcsSystems(_world).Add(new BusinessSetupSystem()).Add(new MoneySystem()).Add(new LevelUpSystem()).Add(new UpdateUISystem()).Add(new IncomeProgressUpdateSystem()).Add(new UpgradeSystem()).Inject(businessConfigSetup).Inject(staticData)
            .Inject(sceneData).ConvertScene();
        _gameplaySystems.Init();
        
        _saveSystems = new EcsSystems(_world).Add(new SaveSystem());
        _saveSystems.Init();
    }
    
    private void Update () {
        _gameplaySystems.Run();
    }

    private void OnDestroy()
    {
        _gameplaySystems.Destroy();
        _world.Destroy();
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause) _saveSystems.Run();
    }
}