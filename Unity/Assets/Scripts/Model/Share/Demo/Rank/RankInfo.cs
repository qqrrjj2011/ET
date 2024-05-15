using MemoryPack;

namespace ET
{
   [MemoryPackable]
    [ChildOf]
    public partial class  RankInfo : Entity,IAwake,IDestroy
    {
        public long   UnitId;
        public string Name;
        public int    Count;
    }
  
}