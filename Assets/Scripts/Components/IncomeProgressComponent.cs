using System;
using UnityEngine.UI;

namespace Components
{
    [Serializable]
    public struct IncomeProgressComponent
    {
        public float currentTime;
        public Slider incomeProgressSlider;
    }
}