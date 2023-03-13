using Components;
using Events;
using Leopotam.Ecs;

namespace CoreSystems
{
    public class LevelUpSystem : IEcsRunSystem
    {
        private EcsWorld _ecsWorld;
        private EcsFilter<BusinessComponent, BusinessTextComponent, LevelUpComponent, IncomeProgressComponent, LevelUpEvent> _levelUpFilter;
    
        public void Run()
        {
            foreach (var idx in _levelUpFilter)
            {
                ref BusinessComponent businessComponent = ref _levelUpFilter.Get1(idx);
                LevelUpBusiness(ref businessComponent);
            
                _levelUpFilter.GetEntity(idx).Del<LevelUpEvent>();
            } 
        }

        private void LevelUpBusiness(ref BusinessComponent businessComponent)
        {
            businessComponent.currentLevel += 1;
        }
    }
}