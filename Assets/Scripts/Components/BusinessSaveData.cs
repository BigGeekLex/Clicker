using System.Collections.Generic;

namespace Components
{
    [System.Serializable]
    public class BusinessSaveData
    {
        public string name;
        public int currentLevel;
        public List<bool> upgradableDatas;

        public BusinessSaveData(string name, int currentLevel, List<bool> upgradableDatas)
        {
            this.name = name;
            this.currentLevel = currentLevel;
            this.upgradableDatas = upgradableDatas;
        }
    }
}