
namespace ET.Server
{
    [FriendOf(typeof(RankInfo))]
    [FriendOf(typeof(ServerRoleInfo))]
    [FriendOf(typeof(UnitRoleInfo))]
    public static class RankHelper
    {
        public static void AddOrUpdateLevelRank(Unit unit)
        {
            // using (RankInfo rankInfo = unit.Root().AddChild<RankInfo>())
            // {
            //     rankInfo.UnitId = unit.Id;
            //     rankInfo.Name   = unit.GetComponent<UnitRoleInfo>().Name;
            //     rankInfo.Count  = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Level);
            //     
            //     Map2Rank_AddOrUpdateRankInfo message = Map2Rank_AddOrUpdateRankInfo.Create();
            //     message.RankInfo = rankInfo;
            //     ActorId instanceId  = StartSceneConfigCategory.Instance.GetBySceneName(unit.Zone(), "Rank").ActorId;
            //     unit.Root().GetComponent<MessageSender>().Send(instanceId, message);
            // }
            
            Map2Rank_AddOrUpdateRankInfo message = Map2Rank_AddOrUpdateRankInfo.Create();
            message.unitId = unit.Id;
            message.roleName   = unit.GetComponent<UnitRoleInfo>().Name;
            // 测试数据
            // int level = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Level);
            // unit.GetComponent<NumericComponent>().Set(NumericType.Level,level+1);
            // level = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Level);
            
            message.count  = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Level);
            ActorId instanceId  = StartSceneConfigCategory.Instance.GetBySceneName(unit.Zone(), "Rank").ActorId;
            unit.Root().GetComponent<MessageSender>().Send(instanceId, message);
        }
    }
 
}