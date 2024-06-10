using System.Net;

namespace ET.Server
{
    [Invoke((long)SceneType.UnitCache)]
    public class FiberInit_UnitCache: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<MessageSender>();
            
            root.AddComponent<DBManagerComponent>();
            root.AddComponent<UnitCacheComponent>();
            
            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.Get(root.Fiber.Id);
            root.AddComponent<NetComponent, IPEndPoint, NetworkProtocol>(startSceneConfig.InnerIPPort, NetworkProtocol.UDP);

            // long roleId = IdGenerater.Instance.GenerateUnitId(1);
            // Log.Warning(">>>>>>roleId:"+roleId);
            // int zoneId = UnitIdStruct.GetUnitZone(roleId);
            // Log.Warning(">>>>>>zoneId:"+zoneId);
            
            await ETTask.CompletedTask;
        }
    }
}