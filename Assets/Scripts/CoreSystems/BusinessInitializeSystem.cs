using System.Collections.Generic;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace CoreSystems
{
    public class BusinessSetupSystem : IEcsPreInitSystem, IEcsInitSystem
    {
        private EcsWorld _ecsWorld;
    
        private EcsFilter<BusinessComponent, BusinessTextComponent, LevelUpComponent, IncomeProgressComponent, UpgradableGroupComponent> _filter;
        private EcsFilter<UpgradableUIComponent> _upgradableTextFilter;
        private BusinessConfigSetup _businessConfigSetup;
    
        private StaticData _staticData;
        private SceneData _sceneData;

        private List<GameObject> _upgradablesGo = new List<GameObject>();
    
        public void PreInit()
        {
            SpawnBusinessInstances();
        }
    
        public void Init()
        {
            InitializeBusinessComponent();
        }
        private void SpawnBusinessInstances()
        {
            for (int i = 0; i < _businessConfigSetup.businessDatas.Count; i++)
            {
                GameObject.Instantiate(_staticData.businessUIPrefab, Vector3.zero, Quaternion.identity, _sceneData.businessParent);
            
                SpawnUpgradables(_businessConfigSetup.businessDatas[i].upgradables.Count); 
            }
        }
        private void SpawnUpgradables(int count)
        {
            for (int j = 0; j < count; j++)
            {
                GameObject nextUpgradablesGo = GameObject.Instantiate(_staticData.upgradeButton, Vector3.zero, Quaternion.identity);
                _upgradablesGo.Add(nextUpgradablesGo);
            }
        }
        private void InitializeBusinessComponent()
        {
            int nextUpgradablesGroupStartIndex = 0;
        
            foreach (var idx in _filter)
            {
                int currentIndex = (_filter.GetEntitiesCount() - 1) - idx;
            
                ref BusinessComponent businessComponent = ref _filter.Get1(idx);
                ref UpgradableGroupComponent upgradableGroupComponent = ref _filter.Get5(idx);
            
                businessComponent.name = _businessConfigSetup.businessDatas[currentIndex].name;
                businessComponent.costBase = _businessConfigSetup.businessDatas[currentIndex].costBase;
                businessComponent.incomeDelay = _businessConfigSetup.businessDatas[currentIndex].incomeDelay;
                businessComponent.incomeBase = _businessConfigSetup.businessDatas[currentIndex].incomeBase;
            
                var upgradableDataScriptableObject = _businessConfigSetup.businessDatas[currentIndex].upgradables;
            
                upgradableGroupComponent.upgradables = InitializeUpgradableData(upgradableDataScriptableObject);

                businessComponent.upgradableGroup = upgradableGroupComponent;
            
                int stepInGroup = upgradableGroupComponent.upgradables.Length - 1;

                int upgradablesCount = upgradableGroupComponent.upgradables.Length;
            
                SetParentForUpgradablesGameObjects(upgradablesCount, nextUpgradablesGroupStartIndex, upgradableGroupComponent.buttonParent);
            
                for (int j = nextUpgradablesGroupStartIndex; j < nextUpgradablesGroupStartIndex + upgradableGroupComponent.upgradables.Length;j++)
                {
                    ref UpgradableUIComponent upgradableUIComponent = ref _upgradableTextFilter.Get1(j);
                
                    InitializeUpgradableUI(upgradableGroupComponent, upgradableUIComponent, stepInGroup);
                
                    stepInGroup--;
                }

                nextUpgradablesGroupStartIndex += upgradablesCount;
            }
        }
        private UpgradableData[] InitializeUpgradableData(List<UpgradableConfigData> data)
        {
            List<UpgradableData> upgradableDatas = new List<UpgradableData>();
            for (int x = 0; x < data.Count; x++)
            {
                UpgradableData upgradableData = new UpgradableData(data[x].name, data[x].cost, data[x].incomeModifier, false);
                upgradableDatas.Add(upgradableData);
            }
            return upgradableDatas.ToArray();
        }
        private void SetParentForUpgradablesGameObjects(int upgradablesCount, int startIndex, Transform parent)
        {
            int replaceIndex = startIndex + upgradablesCount - 1;
            int upgradablesGameObjectsAmount = _upgradablesGo.Count - 1;
        
            for (int i = startIndex; i < startIndex + upgradablesCount; i++)
            {
                _upgradablesGo[upgradablesGameObjectsAmount - replaceIndex].transform.SetParent(parent);
                replaceIndex--;
            }
        }
        private void InitializeUpgradableUI(UpgradableGroupComponent upgradableGroupComponent, UpgradableUIComponent uiComponent, int index)
        {
            upgradableGroupComponent.upgradables[index].actionButton = uiComponent.actionButton;
            upgradableGroupComponent.upgradables[index].upgradableUIComponent = uiComponent;
        }
    }   
}
