using System;
using TMPro;
using UnityEngine.UI;

namespace Components
{
    [Serializable]
    public struct UpgradableUIComponent
    {
        public TextMeshProUGUI nameField;
        public TextMeshProUGUI incomeModifierField;
        public TextMeshProUGUI statusField;
        public Button actionButton;
    }
}