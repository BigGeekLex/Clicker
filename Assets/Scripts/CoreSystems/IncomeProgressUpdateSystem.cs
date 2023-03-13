using Components;
using Events;
using Leopotam.Ecs;
using UnityEngine;

namespace CoreSystems
{
    public class IncomeProgressUpdateSystem : IEcsRunSystem
    {
        private EcsWorld _ecsWorld;
        private EcsFilter<BusinessComponent, BusinessTextComponent, LevelUpComponent, IncomeProgressComponent, UpgradableGroupComponent> _filter;
    
        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref BusinessComponent businessComponent = ref _filter.Get1(idx);
            
                if (businessComponent.IsAvaliable())
                {
                    ref IncomeProgressComponent incomeProgressComponent = ref _filter.Get4(idx);
                
                    if (incomeProgressComponent.currentTime >= businessComponent.incomeDelay)
                    {
                        incomeProgressComponent.currentTime = 0.0f;
                        SendAddMoneyRequest(_filter.GetEntity(idx));
                    }
                    else
                    {
                        incomeProgressComponent.currentTime += Time.deltaTime;
                    }

                    incomeProgressComponent.incomeProgressSlider.value = incomeProgressComponent.currentTime / businessComponent.incomeDelay;
                }
            }
        }

        private void SendAddMoneyRequest(EcsEntity recievedEntity)
        {
            recievedEntity.Get<AddMoneyEvent>();
        }
    }
}