using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof(MoveComponent))]
    [FriendOf(typeof(NumericComponent))]
    [FriendOf(typeof(TimeInfo))]
    [FriendOf(typeof(UnitRoleInfo))]
    [FriendOf(typeof(ServerRoleInfo))]
    public static partial class UnitHelper
    {
        public static UnitInfo CreateUnitInfo(Unit unit)
        {
            UnitInfo unitInfo = UnitInfo.Create();
            NumericComponent nc = unit.GetComponent<NumericComponent>();
            unitInfo.UnitId = unit.Id;
            unitInfo.ConfigId = unit.ConfigId;
            unitInfo.Type = (int)unit.Type();
            unitInfo.Position = unit.Position;
            unitInfo.Forward = unit.Forward;

            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            if (moveComponent != null)
            {
                if (!moveComponent.IsArrived())
                {
                    unitInfo.MoveInfo = MoveInfo.Create();
                    unitInfo.MoveInfo.Points.Add(unit.Position);
                    for (int i = moveComponent.N; i < moveComponent.Targets.Count; ++i)
                    {
                        float3 pos = moveComponent.Targets[i];
                        unitInfo.MoveInfo.Points.Add(pos);
                    }
                }
            }

            foreach ((int key, long value) in nc.NumericDic)
            {
                unitInfo.KV.Add(key, value);
            }

            return unitInfo;
        }
        
        // 获取看见unit的玩家，主要用于广播
        public static Dictionary<long, EntityRef<AOIEntity>> GetBeSeePlayers(this Unit self)
        {
            return self.GetComponent<AOIEntity>().GetBeSeePlayers();
        }
        
        
        public static async ETTask<(bool, Unit)> LoadUnit(Player player)
        {
            //111 GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
            // gateMapComponent.Scene            = await SceneFactory.Create(gateMapComponent, "GateMap", SceneType.Map);
            
            GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
            gateMapComponent.Scene = await GateMapFactory.Create(gateMapComponent, player.Id, IdGenerater.Instance.GenerateInstanceId(), "GateMap");
            
            Log.Warning(">>>>>>player.UnitId:"+player.UnitId);
            Unit unit = await UnitCacheHelper.GetUnitCache(gateMapComponent.Scene, player.UnitId);
            
            bool isNewUnit = unit == null;
            if (isNewUnit)
            {
                unit =  UnitFactory.Create(gateMapComponent.Scene, player.UnitId, UnitType.Player);
                Log.Warning(">>>>>>player.UnitId:"+player.UnitId);
                Log.Warning(">>>>>>player.Zone():"+player.Zone());
                int realmZone = player.Zone() - 1 + 1000;     // 转换成公共服
                var roleInfos = await player.Root().GetComponent<DBManagerComponent>().GetZoneDB(realmZone).Query<ServerRoleInfo>(d => d.Id == player.UnitId);
                UnitRoleInfo unitRoleInfo = new UnitRoleInfo()
                {
                    Name = roleInfos[0].Name,
                    Account = roleInfos[0].Account
                };
                unit.AddComponent(unitRoleInfo);
                
                UnitCacheHelper.AddOrUpdateUnitAllCache(unit);
            }
           

            return (isNewUnit, unit);
        }
        
        public static async ETTask InitUnit(Unit unit, bool isNew)
        {
            unit.GetComponent<NumericComponent>().SetNoEvent(NumericType.BattleRandomSeed,TimeInfo.Instance.ServerNow());
            await ETTask.CompletedTask;
        }
    }
}