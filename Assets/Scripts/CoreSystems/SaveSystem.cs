using System.Collections.Generic;
using System.IO;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace CoreSystems
{
    public class SaveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;
    
        private EcsFilter<BusinessComponent, BusinessTextComponent, LevelUpComponent, IncomeProgressComponent, UpgradableGroupComponent> _businessFilter;
        private EcsFilter<BalanceComponent> _balanceFilter;
        
        private readonly string _businessFilePath = Application.persistentDataPath + "/BusinessData.json";
        private readonly string _balanceFilePath = Application.persistentDataPath + "/Balance.json";
    
        public void Init()
        {
            InitializeBusinessData();
            
            foreach (var idx in _balanceFilter)
            {
                ref BalanceComponent balanceComponent = ref _balanceFilter.Get1(idx);
                balanceComponent.currentBalance = GetBalanceData().currentBalance;
            }
        }
        public void Run()
        {
            foreach (var idx in _balanceFilter)
            {
                ref BalanceComponent balanceComponent = ref _balanceFilter.Get1(idx);
                SaveBalanceData(balanceComponent);
            }
        
            List<BusinessSaveData> saveDatas = new List<BusinessSaveData>();
            
            foreach (var idx in _businessFilter)
            {
                ref BusinessComponent businessComponent = ref _businessFilter.Get1(idx);

                List<bool> upgradesStatusInfo = new List<bool>();

                for (int i = 0; i < businessComponent.upgradableGroup.upgradables.Length; i++)
                {
                    upgradesStatusInfo.Add(businessComponent.upgradableGroup.upgradables[i].isAvaliable);   
                }
            
                BusinessSaveData data = new BusinessSaveData(businessComponent.name, businessComponent.currentLevel, upgradesStatusInfo);
                saveDatas.Add(data);
            } 
        
            SaveBusinessData(saveDatas);
        }
        
        private void InitializeBusinessData()
        {
            BusinessSaveData[] savedData = GetBusinessSaveData();
        
            if(savedData == null) return;
        
            if (savedData.Length > 0)
            {
                foreach (var idx in _businessFilter)
                {
                    ref BusinessComponent businessComponent = ref _businessFilter.Get1(idx);

                    for (int i = 0; i < savedData.Length; i++)
                    {
                        if (savedData[i].name == businessComponent.name)
                        {
                            businessComponent.currentLevel = savedData[i].currentLevel;
                        
                            for (int j = 0; j < businessComponent.upgradableGroup.upgradables.Length; j++)
                            {
                            
                                if(savedData[i].upgradableDatas == null) continue;
                            
                                businessComponent.upgradableGroup.upgradables[j].isAvaliable = savedData[i].upgradableDatas[j];
                            }
                        }
                    }
                }   
            }
        }
        private void SaveBusinessData(List<BusinessSaveData> businessSaveData)
        {
            string data = JsonHelper.ToJson(businessSaveData.ToArray());
            File.WriteAllText(_businessFilePath, data);
        }
    
        private void SaveBalanceData(BalanceComponent value)
        {
            string balanceData = JsonUtility.ToJson(value);
            File.WriteAllText(_balanceFilePath, balanceData);
        }
        private BalanceComponent GetBalanceData()
        {
            string data;
            if (!File.Exists(_balanceFilePath))
            {
                File.Create(_balanceFilePath);
                TextAsset textAsset = Resources.Load<TextAsset>("Balance");
                data = textAsset.text;
            }
            else
            {
                data = File.ReadAllText(_balanceFilePath);   
            }
            return JsonUtility.FromJson<BalanceComponent>(data);
        }
        private BusinessSaveData[] GetBusinessSaveData()
        {
            string data;
            
            if (!File.Exists(_businessFilePath))
            {
                File.Create(_businessFilePath);
                TextAsset textAsset = Resources.Load<TextAsset>("BusinessData");
                data = textAsset.text;
            }
            else
            {
               data = File.ReadAllText(_businessFilePath);    
            }
            
            return JsonHelper.FromJson<BusinessSaveData>(data);
        }
    }
}