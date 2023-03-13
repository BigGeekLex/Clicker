using Components;
using Events;
using Leopotam.Ecs;

namespace CoreSystems
{
    public class MoneySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;
    
        private EcsFilter<BusinessComponent, BusinessTextComponent, LevelUpComponent, IncomeProgressComponent, UpgradableGroupComponent> _filter;
    
        private EcsFilter<BusinessComponent, BusinessTextComponent, LevelUpComponent, IncomeProgressComponent, UpgradableGroupComponent, AddMoneyEvent> _addMoneyFilter;
    
        private EcsFilter<BalanceComponent> _balance;
    
        public void Init()
        {
            EcsEntity balanceEntity = _ecsWorld.NewEntity();
            balanceEntity.Get<BalanceComponent>();
        
            foreach (var idx in _filter)
            {
                ref LevelUpComponent component = ref _filter.Get3(idx);
                ref UpgradableGroupComponent upgradableGroupComponent = ref _filter.Get5(idx);
            
                component.button.onClick.AddListener(() => OnLevelUpBuyReqeust(_filter.GetEntity(idx)));
                
                for (int i = 0; i <  upgradableGroupComponent.upgradables.Length; i++)
                {
                    int index = i;
                    upgradableGroupComponent.upgradables[i].actionButton.onClick.AddListener(() => OnUpgradeBuyRequest(_filter.GetEntity(idx), index));
                }
            }
        }
    
        public void Run()
        {
            foreach (var idx in _addMoneyFilter)
            {
                _addMoneyFilter.GetEntity(idx).Del<AddMoneyEvent>();
                ref BusinessComponent businessComponent = ref _addMoneyFilter.Get1(idx);
                AddBalance(businessComponent.GetNextIncome());
            }
        }
        private void OnLevelUpBuyReqeust(EcsEntity recievedEntity)
        {
            ref BusinessComponent businessComponent = ref recievedEntity.Get<BusinessComponent>();
            BuyLevelUp(businessComponent, recievedEntity);
        }
    
        private void OnUpgradeBuyRequest(EcsEntity receivedEntity, int index)
        {
            BuyUpgrade(receivedEntity, index);
        }
        private void BuyLevelUp(BusinessComponent component, EcsEntity recivedEntity)
        {
            if (TryBuy(component.GetLevelUpCost()))
            {
                SendLevelUpRequest(recivedEntity);
            }
        }
        private void BuyUpgrade(EcsEntity receivedEntity, int index)
        {
            ref BusinessComponent businessComponent = ref receivedEntity.Get<BusinessComponent>();

            if (!businessComponent.IsAvaliable() || receivedEntity.Get<UpgradableGroupComponent>().upgradables[index].isAvaliable) return;
        
            if (TryBuy(receivedEntity.Get<UpgradableGroupComponent>().upgradables[index].cost))
            {
                SendUpgradeRequest(receivedEntity, index);
            }
        }
        private bool TryBuy(int price)
        {
            foreach (var idx in _balance)
            {
                ref BalanceComponent balanceComponent = ref _balance.Get1(idx);
            
                if (balanceComponent.currentBalance >= price)
                {
                    balanceComponent.currentBalance -= price;
                    return true;
                }
            }
            return false;
        }
        private void AddBalance(int value)
        {
            foreach (var idx in _balance)
            {
                ref BalanceComponent balanceComponent = ref _balance.Get1(idx);
                balanceComponent.currentBalance += value;
            }
        }
        private void SendLevelUpRequest(EcsEntity receivedEntity)
        {
            receivedEntity.Get<LevelUpEvent>();
        }
        private void SendUpgradeRequest(EcsEntity receivedEntity, int index)
        {
            ref UpgradeEvent upgradeEvent = ref receivedEntity.Get<UpgradeEvent>();
            upgradeEvent.index = index;
        }
    }
}