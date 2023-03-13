using System;

namespace Components
{
    [Serializable]
    public struct BusinessComponent
    {
        public string name;
        public float incomeDelay;
        public int incomeBase;
        public int costBase;
        public int currentLevel;

        public UpgradableGroupComponent upgradableGroup;
        public int GetNextIncome()
        {
            return (int)(currentLevel * incomeBase * (1 + upgradableGroup.GetUpgradesIncomeModifier()));
        }
        public int GetLevelUpCost()
        {
            return costBase * (currentLevel + 1);
        }
        public bool IsAvaliable()
        {
            return currentLevel > 0;
        }
    }
}