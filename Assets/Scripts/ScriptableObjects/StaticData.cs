using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Configs/StaticData", order = 1)]
public class StaticData : ScriptableObject
{
    public GameObject businessUIPrefab;
    public GameObject upgradeButton;
}