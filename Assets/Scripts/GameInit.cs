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
    private EcsSystems _systems;

    private void Awake()
    {
        _world = new EcsWorld ();
        _systems = new EcsSystems(_world).Add(new BusinessSetupSystem()).Add(new MoneySystem()).Add(new LevelUpSystem()).Add(new UpdateUISystem()).Add(new IncomeProgressUpdateSystem()).Add(new UpgradeSystem()).Inject(businessConfigSetup).Inject(staticData)
            .Inject(sceneData).ConvertScene().Add(new SaveSystem());
        _systems.Init();
    }
    
    private void Update () {
        _systems.Run();
    }

    private void OnDestroy()
    {
        _systems.Destroy();
    }
}