using System.Net;

namespace ET.Server
{
    [Invoke((long)SceneType.LoginCenter)]
    public class FiberInit_LoginCenter:AInvokeHandler<FiberInit,ETTask>
    {
        public override async ETTask Handle(FiberInit args)
        {
            Log.Info(">>>>>>>firber init logincenter");
            Scene root = args.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<MessageSender>();
            
            root.AddComponent<LoginInfoRecordComponent>();

            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.Get((int)root.Id);
            root.AddComponent<NetComponent, IPEndPoint, NetworkProtocol>(startSceneConfig.InnerIPPort, NetworkProtocol.UDP);
            await ETTask.CompletedTask;
        }
    }
}

