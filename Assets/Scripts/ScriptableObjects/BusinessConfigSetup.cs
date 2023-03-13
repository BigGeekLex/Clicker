using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Config", menuName = "Configs/Business", order = 1)]
public class BusinessConfigSetup : ScriptableObject
{
    public List<BusinessConfigData> businessDatas = new List<BusinessConfigData>();
}

[System.Serializable]
public class BusinessConfigData
{
    [Header("Наименование бизнеса")]
    public string name;
    [Header("Базовая стоимость")]
    public int costBase;
    [Header("Задержка дохода")]
    public float incomeDelay;
    [Header("Базовый доход")]
    public int incomeBase;
    [Header("Улучшения")]
    public List<UpgradableConfigData> upgradables = new List<UpgradableConfigData>();
}

[System.Serializable]
public class UpgradableConfigData
{
    [Header("Наименование улучшения")]
    public string name;
    [Header("Стоимость улучшения")]
    public int cost;
    [Header("Множитель дохода")]
    public float incomeModifier;
}
