using System.Collections.Generic;

namespace ET.Client
{
   // [ChildOf(typeof(RankInfo))]
    [ComponentOf(typeof(Scene))]
    public class RankComponent : Entity,IAwake,IDestroy
    {
        public List<EntityRef<RankInfo>> RankInfos = new List<EntityRef<RankInfo>>(100);
    }
}