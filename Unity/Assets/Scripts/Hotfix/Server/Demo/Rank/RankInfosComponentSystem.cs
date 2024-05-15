using System.Linq;

namespace ET.Server
{
    [FriendOf(typeof(RankInfosComponent))]
    public class RankInfosComponentDestroy : DestroySystem<RankInfosComponent>
    {
        protected override void Destroy(RankInfosComponent self)
        {
            foreach (RankInfo rankInfo in self.RankInfosDictionary.Values)
            {
                rankInfo?.Dispose();
            }
            self.RankInfosDictionary.Clear();
            self.SortedRankInfoList.Clear();
        }
    }

    [FriendOf(typeof(RankInfo))]
    [FriendOf(typeof(RankInfosComponent))]
    public static class RankInfosComponentSystem
    {
        public static async ETTask LoadRankInfo(this RankInfosComponent self)
        {
            var rankInfoList = await self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Query<RankInfo>(d => true,collection:"RankInfosComponent");
            
            foreach ( RankInfo rankInfo in rankInfoList )
            {
                self.AddChild(rankInfo);
                self.RankInfosDictionary.Add(rankInfo.UnitId,rankInfo);
                self.SortedRankInfoList.Add(rankInfo,rankInfo.UnitId);
            }
        }
        
        public static void AddOrUpdate(this RankInfosComponent self, Map2Rank_AddOrUpdateRankInfo message)
        {
            if ( self.RankInfosDictionary.ContainsKey(message.unitId) )
            {
                RankInfo oldRankInfo = self.RankInfosDictionary[message.unitId];
                if (oldRankInfo.Count == message.count)
                {
                    return;
                }
                
                self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Remove<RankInfo>(oldRankInfo.UnitId,oldRankInfo.Id,"RankInfosComponent").Coroutine();
                self.RankInfosDictionary.Remove(oldRankInfo.UnitId);
                self.SortedRankInfoList.Remove(oldRankInfo);
                oldRankInfo?.Dispose();
            }

          
            RankInfo rankInfo = self.AddChild<RankInfo>();
            rankInfo.UnitId = message.unitId;
            rankInfo.Name = message.roleName;
            rankInfo.Count = message.count;
            self.RankInfosDictionary.Add(rankInfo.UnitId,rankInfo);
            self.SortedRankInfoList.Add(rankInfo,rankInfo.UnitId);
            self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Save(rankInfo.UnitId,rankInfo,"RankInfosComponent").Coroutine();
        }
    }
}