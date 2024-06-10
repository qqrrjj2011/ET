using System.Collections.Generic;

namespace ET.Server
{

    [ChildOf]
    public class UnitCache : Entity,IAwake,IDestroy
    {
        public string key;

        public Dictionary<long, EntityRef<Entity>> CacheCompoenntsDictionary = new Dictionary<long, EntityRef<Entity>>();
    }
}