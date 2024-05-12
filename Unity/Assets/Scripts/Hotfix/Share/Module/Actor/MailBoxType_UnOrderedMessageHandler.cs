namespace ET
{
    [Invoke((long)MailBoxType.UnOrderedMessage)]
    public class MailBoxType_UnOrderedMessageHandler: AInvokeHandler<MailBoxInvoker>
    {
        public override void Handle(MailBoxInvoker args)
        {
            HandleAsync(args).Coroutine();
        }
        
        private static async ETTask HandleAsync(MailBoxInvoker args)
        {
            MailBoxComponent mailBoxComponent = args.MailBoxComponent;
            
            MessageObject messageObject = args.MessageObject;
            Log.Warning($">>>>>>HandleAsync  mailBoxComponent.Parent InstanceId:{mailBoxComponent.Parent.InstanceId} sceneType:{mailBoxComponent.Parent.IScene.SceneType} messageObject:{messageObject}");
            await MessageDispatcher.Instance.Handle(mailBoxComponent.Parent, args.FromAddress, messageObject);
        }
    }
}