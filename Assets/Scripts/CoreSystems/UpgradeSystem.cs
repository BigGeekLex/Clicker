using Components;
using Events;
using Leopotam.Ecs;

namespace CoreSystems
{
    public class UpgradeSystem : IEcsRunSystem
    {
        private EcsWorld _ecsWorld;
        private EcsFilter<BusinessComponent, BusinessTextComponent, LevelUpComponent, IncomeProgressComponent, UpgradableGroupComponent, UpgradeEvent> _filter;
    
        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref UpgradeEvent upgradeEvent = ref _filter.Get6(idx);
                ref UpgradableGroupComponent upgradableGroupComponent = ref _filter.Get5(idx);

                for (int i = 0; i < upgradableGroupComponent.upgradables.Length; i++)
                {
                    if (i == upgradeEvent.index)
                    {
                        upgradableGroupComponent.upgradables[i].isAvaliable = true;
                    }
                }
                _filter.GetEntity(idx).Del<UpgradeEvent>();
            }
        }
    }
}