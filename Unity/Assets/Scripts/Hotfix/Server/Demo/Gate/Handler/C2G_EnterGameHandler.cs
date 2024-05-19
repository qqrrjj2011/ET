using System;

namespace ET.Server
{
    [FriendOf(typeof(SessionStateComponent))]
    [FriendOf(typeof(SessionPlayerComponent))]
    [FriendOf(typeof(UnitRoleInfo))]
    [FriendOf(typeof(MessageSender))]
    // [FriendClassAttribute(typeof(ET.RoleInfo))]
   //  [FriendClassAttribute(typeof(ET.UnitGateComponent))]
    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_EnterGameHandler : MessageSessionHandler<C2G_EnterGame, G2C_EnterGame>
    {
        protected override async ETTask Run(Session session, C2G_EnterGame request, G2C_EnterGame response)
        {
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
               // reply();
                return;
            }

            SessionPlayerComponent sessionPlayerComponent = session.GetComponent<SessionPlayerComponent>();
            if (null == sessionPlayerComponent)
            {
                response.Error = ErrorCode.ERR_SessionPlayerError;
                //reply();
                return;
            }

            Player player = session.Root().GetComponent<PlayerComponent>().Get(sessionPlayerComponent.Player.Account) as Player;
            if (player == null || player.IsDisposed)
            {
                response.Error = ErrorCode.ERR_NonePlayerError;
               // reply();
                return;
            }

            long instanceId = session.InstanceId;

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await session.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.LoginGate, player.Account.GetHashCode()))
                {

                    if (instanceId != session.InstanceId || player.IsDisposed)
                    {
                        response.Error = ErrorCode.ERR_PlayerSessionError;
                       // reply();
                        return;
                    }

                    if (session.GetComponent<SessionStateComponent>() != null && session.GetComponent<SessionStateComponent>().State == SessionState.Game)
                    {
                        response.Error = ErrorCode.ERR_SessionStateError;
                       // reply();
                        return;
                    }

                    if (player.PlayerState == PlayerState.Game)
                    {
                        try
                        {
                           //111原来写法 IActorResponse reqEnter = await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestEnterGameState());
                            M2G_RequestEnterGameState reqEnter = await session.Root().GetComponent<MessageSender>().Call(player.GetActorId(), G2M_RequestEnterGameState.Create()) as M2G_RequestEnterGameState;
                            if (reqEnter.Error == ErrorCode.ERR_Success)
                            {
                              //  reply();
                                return;
                            }
                            Log.Error("二次登录失败  " + reqEnter.Error + " | " + reqEnter.Message);
                            response.Error = ErrorCode.ERR_ReEnterGameError;
                            await DisconnectHelper.KickPlayer(player, true);
                           // reply();
                            session?.Disconnect().Coroutine();
                        }
                        catch (Exception e)
                        {
                            Log.Error("二次登录失败  " + e.ToString());
                            response.Error = ErrorCode.ERR_ReEnterGameError2;
                            await DisconnectHelper.KickPlayer(player, true);
                           // reply();
                            session?.Disconnect().Coroutine();
                            throw;
                        }
                        return;
                    }

                    try
                    {

                        //GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
                        //gateMapComponent.Scene = await SceneFactory.Create(gateMapComponent, "GateMap", SceneType.Map);

                        //从数据库或者缓存中加载出Unit实体及其相关组件
                        (bool isNewPlayer, Unit unit) = await UnitHelper.LoadUnit(player);
                        
                       //111 unit.AddComponent<UnitGateComponent, long>(session.InstanceId);
                       //222 unit.AddComponent<UnitGateComponent, long>(player.ClientSession.InstanceId);
                        unit.AddComponent<UnitGateComponent, ActorId>(player.GetComponent<PlayerSessionComponent>().gateSession.GetActorId());
                        
                        Log.Warning(">>>>>playerId 02:"+player.InstanceId);
                        player.ChatInfoActorId = await this.EnterWorldChatServer(unit,player); //登录聊天服
                        

                        //玩家Unit上线后的初始化操作
                        await UnitHelper.InitUnit(unit, isNewPlayer);
                        response.MyId = unit.Id;
                     
                        SessionStateComponent SessionStateComponent = session.GetComponent<SessionStateComponent>();
                        if (SessionStateComponent == null)
                        {
                            SessionStateComponent = session.AddComponent<SessionStateComponent>();
                        }
                        SessionStateComponent.State = SessionState.Game;
                        player.PlayerState = PlayerState.Game;
                        
                        StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.Zone(), "Map1");
                        TransferHelper.TransferAtFrameFinish(unit, startSceneConfig.ActorId, startSceneConfig.Name).Coroutine();
                    }
                    catch (Exception e)
                    {

                        Log.Error($"角色进入游戏逻辑服出现问题 账号Id: {player.Account}  角色Id: {player.Id}   异常信息： {e.ToString()}");
                        response.Error = ErrorCode.ERR_EnterGameError;
                       // reply();
                        await DisconnectHelper.KickPlayer(player, true);
                        session.Disconnect().Coroutine();

                    }
                }
            }
        }
        
        private async ETTask<ActorId> EnterWorldChatServer(Unit unit,Player player)
        {
            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(unit.Zone(), "ChatInfo");

            G2Chat_EnterChat g2ChatEnterChat = G2Chat_EnterChat.Create();
            g2ChatEnterChat.UnitId = unit.Id;
            g2ChatEnterChat.Name = unit.GetComponent<UnitRoleInfo>().Name;
           // g2ChatEnterChat.GateSessionActorId = unit.GetComponent<UnitGateComponent>().GateSessionActorId;
            g2ChatEnterChat.PlayerSessionComponentActorId = player.GetComponent<PlayerSessionComponent>().GetActorId();
            Chat2G_EnterChat chat2GEnterChat = (Chat2G_EnterChat)await unit.Root().GetComponent<MessageSender>().Call(startSceneConfig.ActorId,g2ChatEnterChat);
            return chat2GEnterChat.ChatInfoUnitActorId;
        }
    }
}