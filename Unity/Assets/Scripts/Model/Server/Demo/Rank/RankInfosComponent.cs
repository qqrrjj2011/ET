using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
   // [ChildOf(typeof(RankInfo))]
    [ComponentOf(typeof(Scene))]
    public class RankInfosComponent : Entity,IAwake,IDestroy
    {
        [BsonIgnore]
        public SortedList<EntityRef<RankInfo>, long> SortedRankInfoList  = new SortedList< EntityRef<RankInfo>, long>(new RankInfoCompare());
        
        [BsonIgnore]
        public Dictionary<long, EntityRef<RankInfo>> RankInfosDictionary = new Dictionary<long, EntityRef<RankInfo>>();
    }
    
    
    [EnableClass]
    [FriendOf(typeof(RankInfo))]
    public class RankInfoCompare : IComparer<EntityRef<RankInfo>>
    {
        public int Compare(EntityRef<RankInfo> aa, EntityRef<RankInfo> bb)
        {
            RankInfo a = aa;
            RankInfo b = bb;
            int result = b.Count - a.Count;

            if (result != 0)
            {
                return result;
            }
            
            if (a.Id < b.Id)
            {
                return 1;
            }

            if (a.Id > b.Id)
            {
                return -1;
            }
            return 0;
        }
    }
    
   
}