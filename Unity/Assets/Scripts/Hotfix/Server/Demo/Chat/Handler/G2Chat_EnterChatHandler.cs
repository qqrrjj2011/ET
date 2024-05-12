using System;

namespace ET.Server
{
    [FriendOf(typeof(ChatInfoUnit))]
    [MessageHandler(SceneType.ChatInfo)]
    public class G2Chat_EnterChatHandler : MessageHandler<Scene, G2Chat_EnterChat, Chat2G_EnterChat>
    {
        protected override async ETTask Run(Scene scene, G2Chat_EnterChat request, Chat2G_EnterChat response)
        {
            ChatInfoUnitsComponent chatInfoUnitsComponent = scene.GetComponent<ChatInfoUnitsComponent>();

            ChatInfoUnit chatInfoUnit = chatInfoUnitsComponent.Get(request.UnitId);

            if ( chatInfoUnit != null && !chatInfoUnit.IsDisposed )
            {
                chatInfoUnit.Name               = request.Name;
                chatInfoUnit.PlayerSessionComponentActorId = request.PlayerSessionComponentActorId;
                response.ChatInfoUnitActorId = chatInfoUnit.GetActorId();
              //  reply();
                return;
            }
            
            chatInfoUnit      = chatInfoUnitsComponent.AddChildWithId<ChatInfoUnit>(request.UnitId);
           
            // 类型GateSession不太确定，先用这个试试
            chatInfoUnit.AddComponent<MailBoxComponent,MailBoxType>(MailBoxType.UnOrderedMessage);
          //  await chatInfoUnit.AddLocation(LocationType.Chat);
            
            chatInfoUnit.Name               = request.Name;
            chatInfoUnit.PlayerSessionComponentActorId = request.PlayerSessionComponentActorId;
            
            response.ChatInfoUnitActorId = chatInfoUnit.GetActorId();
            response.ChatInfoUnitInstanceID = chatInfoUnit.Id;
            
            Log.Warning($"chatInfoUnit  sceneType：{chatInfoUnit.IScene.SceneType.ToString()}");
            chatInfoUnitsComponent.Add(chatInfoUnit);
            
            Log.Warning($"chatInfoUnitsComponent, from: {chatInfoUnit.GetActorId()} current chatInfoUnit: {chatInfoUnit.Fiber().Address} scene fiber：{scene.Fiber().Address}  actorId.InstanceId：{chatInfoUnit.InstanceId}");
       //     reply();
            await ETTask.CompletedTask;
        }
    }
}