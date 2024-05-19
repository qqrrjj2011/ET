using System;
using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class AdventureCheckComponent : Entity,IAwake,IDestroy
    {
        public int AniamtionTotalTime      = 0;
        
        public List<long> EnemyIdList      = new List<long>();
        public List<long> CacheEnemyIdList = new List<long>();
        
        public SRandom Random              = null;
    }
}