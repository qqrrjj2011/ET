using System.Collections.Generic;

namespace ET.Server
{
    //[ComponentOf(typeof(Scene))]
    //[ChildOf(typeof(UnitCache))]
    [ComponentOf]
    public class UnitCacheComponent : Entity,IAwake,IDestroy
    {
        public Dictionary<string, EntityRef<UnitCache>> UnitCaches = new Dictionary<string, EntityRef<UnitCache>>();

        public List<string> UnitCacheKeyList = new List<string>();

    }
}