using System;
using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof(GeneralsComponent))]
    [MessageHandler(SceneType.Map)]
    [FriendOfAttribute(typeof(ET.OwnerComponent))]
    public class M2M_UnitTransferRequestHandler : MessageHandler<Scene, M2M_UnitTransferRequest, M2M_UnitTransferResponse>
    {
        protected override async ETTask Run(Scene scene, M2M_UnitTransferRequest request, M2M_UnitTransferResponse response)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            Unit unit = MongoHelper.Deserialize<Unit>(request.Unit);

            unitComponent.AddChild(unit);
            unitComponent.Add(unit);

            unit.AddComponent<UnitDBSaveComponent>();

            foreach (byte[] bytes in request.Entitys)
            {
                Entity entity = MongoHelper.Deserialize<Entity>(bytes);
                unit.AddComponent(entity);
            }

            unit.AddComponent<MoveComponent>();
            unit.AddComponent<PathfindingComponent, string>(scene.Name);
            unit.Position = new float3(-10, 0, -10);

            unit.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.OrderedMessage);

            // 创建5个玩家将领
            GeneralsComponent generalsComponent = unit.GetComponent<GeneralsComponent>();
            if (generalsComponent == null) generalsComponent = unit.AddComponent<GeneralsComponent>();
            M2C_CreateGeneralsUnits m2CCreateGenaralsUnits = M2C_CreateGeneralsUnits.Create();
            if (generalsComponent.generalsIds.Count > 0)
            {
                for (int i = 0; i < generalsComponent.generalsIds.Count; i++)
                {
                    Unit generalsUnit = UnitFactory.CreateGenerals(scene, generalsComponent.generalsIds[i], 4000);
                    OwnerComponent ownerComponent = generalsUnit.AddComponent<OwnerComponent>();
                    ownerComponent.ownerId = unit.Id;
                    m2CCreateGenaralsUnits.Units.Add(UnitHelper.CreateUnitInfo(generalsUnit));
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    long generalsId = IdGenerater.Instance.GenerateId();
                    Unit generalsUnit = UnitFactory.CreateGenerals(scene, generalsId, 4000);
                    generalsComponent.generalsIds.Add(generalsId);
                    
                    OwnerComponent ownerComponent = generalsUnit.AddComponent<OwnerComponent>();
                    ownerComponent.ownerId = unit.Id;
                    m2CCreateGenaralsUnits.Units.Add(UnitHelper.CreateUnitInfo(generalsUnit));
                }
            }

            // 通知客户端开始切场景
            M2C_StartSceneChange m2CStartSceneChange = M2C_StartSceneChange.Create();
            m2CStartSceneChange.SceneInstanceId = scene.InstanceId;
            m2CStartSceneChange.SceneName = scene.Name;
            MapMessageHelper.SendToClient(unit, m2CStartSceneChange);

            // 通知客户端创建My Unit
            M2C_CreateMyUnit m2CCreateUnits = M2C_CreateMyUnit.Create();
            m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
            MapMessageHelper.SendToClient(unit, m2CCreateUnits);

            // 通知客服端创建将领
            MapMessageHelper.SendToClient(unit, m2CCreateGenaralsUnits);

            // 通知客户端同步背包信息
            ItemUpdateNoticeHelper.SyncAllBagItems(unit);

            // 通知客户端同步装备
            ItemUpdateNoticeHelper.SyncAllEquipItems(unit);

            // 通知客户端同步打造信息
            ForgeHelper.SyncAllProduction(unit);

            // 通知客户端同步任务信息
            TaskNoticeHelper.SyncAllTaskInfo(unit);

            unit.AddComponent<NumericNoticeComponent>();
            unit.AddComponent<AdventureCheckComponent>();

            // 加入aoi
            //111 unit.AddComponent<AOIEntity, int, float3>(9 * 1000, unit.Position);

            // 添加排行榜测试数据
            RankHelper.AddOrUpdateLevelRank(unit);

            // 解锁location，可以接收发给Unit的消息
            await scene.Root().GetComponent<LocationProxyComponent>().UnLock(LocationType.Unit, unit.Id, request.OldActorId, unit.GetActorId());
        }
    }
}