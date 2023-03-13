using System;
using TMPro;

namespace Components
{
    [Serializable]
    public struct BusinessTextComponent
    {
        public TextMeshProUGUI nameTextField;
        public TextMeshProUGUI currentLevelText;
        public TextMeshProUGUI currentIncomeText;
        public TextMeshProUGUI nextLevelUpPriceText;
    }
}