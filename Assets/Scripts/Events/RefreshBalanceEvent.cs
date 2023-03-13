using Leopotam.Ecs;

namespace Events
{
    public struct RefreshBalanceEvent
    {
        public EcsEntity eventEntity;
        public int CurrentBalance;
    }
}