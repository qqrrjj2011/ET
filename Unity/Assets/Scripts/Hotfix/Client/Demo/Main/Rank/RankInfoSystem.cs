namespace ET.Client
{
    [FriendOf(typeof(RankInfo))]
    public class RankInfoDestroySystem: DestroySystem<RankInfo>
    {
        protected override void Destroy(RankInfo self)
        {
            self.UnitId = 0;
            self.Name   = null;
            self.Count  = 0;
        }
    }

    [FriendOf(typeof(RankInfo))]
    public static class RankInfoSystem
    {
        public static void FromMessage(this RankInfo self, RankInfoProto rankInfoProto)
        {
           // self.Id      = rankInfoProto.Id;
            self.UnitId  = rankInfoProto.UnitId;
            self.Name    = rankInfoProto.Name;
            self.Count   = rankInfoProto.Count;
        }

        public static RankInfoProto ToMessage(this RankInfo self)
        {
            RankInfoProto rankInfoProto = RankInfoProto.Create();
            rankInfoProto.Id     = self.Id;
            rankInfoProto.UnitId = self.UnitId;
            rankInfoProto.Name   = self.Name;
            rankInfoProto.Count  = self.Count;
            return rankInfoProto;
        }
    }
}