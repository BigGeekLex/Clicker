using System;
using UnityEngine.UI;

namespace Components
{
    [Serializable]
    public class UpgradableData
    {
        public string name;
        public bool isAvaliable;
        public int cost;
        public float incomeModifier;
        public Button actionButton;
        public UpgradableUIComponent upgradableUIComponent;

        public UpgradableData(string name, int cost, float incomeModifier, bool isAvaliable)
        {
            this.name = name;
            this.cost = cost;
            this.incomeModifier = incomeModifier;
            this.isAvaliable = isAvaliable;
        }
    }
}