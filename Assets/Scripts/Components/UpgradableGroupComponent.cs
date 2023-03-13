using System;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    [Serializable]
    public struct UpgradableGroupComponent
    {
        public UpgradableData[] upgradables;
        public Transform buttonParent;
        
        public UpgradableUIComponent GetUpgradableUIComponentByIndex(int index)
        {
            return upgradables[index].upgradableUIComponent;
        }
        
        public float GetUpgradesIncomeModifier()
        {
            List<Func<float, float>> mod = new List<Func<float, float>>();

            for (int i = 0; i < upgradables.Length; i++)
            {
                if (upgradables[i].isAvaliable)
                {
                    float incomeModifier = upgradables[i].incomeModifier;
                    mod.Add(x => x + incomeModifier);
                }
            }
            return CalculateValueWithModifier(mod, 0);
        }
        private float CalculateValueWithModifier(List<Func<float, float>> mods, float initialValue)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                initialValue = mods[i].Invoke(initialValue);
            }
            return initialValue;
        }
    }
}