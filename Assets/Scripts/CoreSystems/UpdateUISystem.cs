using Components;
using Leopotam.Ecs;

namespace CoreSystems
{
    public class UpdateUISystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;
    
        private EcsFilter<BalanceComponent> _balance;
        private EcsFilter<BusinessComponent, BusinessTextComponent, LevelUpComponent, IncomeProgressComponent, UpgradableGroupComponent> _initializedFilter;
    
        private SceneData _sceneData;
    
        public void Init()
        {
            foreach (var idx in _initializedFilter)
            {
                ref BusinessComponent businessComponent = ref _initializedFilter.Get1(idx);
                ref BusinessTextComponent businessTextComponent = ref _initializedFilter.Get2(idx);
                ref UpgradableGroupComponent upgradableGroupComponent = ref _initializedFilter.Get5(idx);
            
                businessTextComponent.nameTextField.text = businessComponent.name;
            
                RefreshBusinessUIComponents(businessComponent, businessTextComponent);
                InitializeUpgradablesUIComponents(upgradableGroupComponent);
            }
        }
    
        public void Run()
        {
            foreach (var idx in _balance)
            {
                ref BalanceComponent refreshBalanceEvent = ref _balance.Get1(idx);
                _sceneData.balanceText.text = $"Баланс: {refreshBalanceEvent.currentBalance.ToString()}$";
            }

            foreach (var idx in _initializedFilter)
            {
                ref BusinessComponent businessComponent = ref _initializedFilter.Get1(idx);
                ref BusinessTextComponent businessTextComponent = ref _initializedFilter.Get2(idx);
                ref UpgradableGroupComponent upgradableGroupComponent = ref _initializedFilter.Get5(idx);
            
            
                RefreshBusinessUIComponents(businessComponent, businessTextComponent);
                RefreshUpgradablesUIComponents(upgradableGroupComponent);
            }
        }
        private void RefreshBusinessUIComponents(BusinessComponent businessComponent, BusinessTextComponent textComponent)
        {
            textComponent.currentLevelText.text = $"LVL \n {businessComponent.currentLevel.ToString()}";
            textComponent.nextLevelUpPriceText.text = $"Цена: {businessComponent.GetLevelUpCost().ToString()} $";
            textComponent.currentIncomeText.text = $"{businessComponent.GetNextIncome()} $";
        }

        private void InitializeUpgradablesUIComponents(UpgradableGroupComponent upgradableGroupComponent)
        {
            for (int i = 0; i < upgradableGroupComponent.upgradables.Length; i++)
            {
                UpgradableUIComponent upgradableUIComponent = upgradableGroupComponent.GetUpgradableUIComponentByIndex(i);
                upgradableUIComponent.nameField.text = upgradableGroupComponent.upgradables[i].name;
                upgradableUIComponent.incomeModifierField.text = $"Доход: +{upgradableGroupComponent.upgradables[i].incomeModifier*100}% ";
                upgradableUIComponent.statusField.text = $"Цена: {upgradableGroupComponent.upgradables[i].cost}";
            }
        }
        private void RefreshUpgradablesUIComponents(UpgradableGroupComponent upgradableGroupComponent)
        {
            for (int i = 0; i < upgradableGroupComponent.upgradables.Length; i++)
            {
                UpgradableUIComponent upgradableUIComponent = upgradableGroupComponent.GetUpgradableUIComponentByIndex(i);
                
                if (upgradableGroupComponent.upgradables[i].isAvaliable)
                {
                    string boughtText = "Куплено";
                    upgradableUIComponent.statusField.text = boughtText;
                }
            }
        }
    }
}
