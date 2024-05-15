namespace ET.Client
{
    [ChildOf(typeof(RankComponent))]
    public class RankInfo : Entity,IAwake,IDestroy
    {
        public long   UnitId;
        public string Name;
        public int    Count;
    }
}