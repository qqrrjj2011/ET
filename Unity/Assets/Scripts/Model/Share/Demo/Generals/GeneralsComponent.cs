using System.Collections.Generic;

namespace ET
{
    [ComponentOf]
    public class GeneralsComponent:Entity,IAwake,ITransfer,IUnitCache
    {
        /// <summary>
        /// 玩家将领Id
        /// </summary>
        public List<long> generalsIds = new List<long>();
    }    
}
