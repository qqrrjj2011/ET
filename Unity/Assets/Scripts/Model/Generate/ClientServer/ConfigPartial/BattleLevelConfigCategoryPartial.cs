 
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public partial class BattleLevelConfigCategory
    {
        private List<BattleLevelConfig> list;
        public BattleLevelConfig GetConfigByIndex(int index)
        {
            this.list ??= this.dict.Values.ToList();

            if (index >= 0 && index < this.list.Count)
            {
                return this.list[index];
            }

            Log.Error($"Get BattleLevelConfig Index Error: {index}");
            return null;
        }
        
     
    }
}