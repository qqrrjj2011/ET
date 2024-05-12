using System;


namespace ET.Server
{
	[FriendOf(typeof(SessionStateComponent))]
	[FriendOf(typeof(SessionLockingComponent))]
	[FriendOf(typeof(SessionPlayerComponent))]
	[FriendOf(typeof(Player))]
    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_LoginGateHandler : MessageSessionHandler<C2G_LoginGate, G2C_LoginGate>
    {
        protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response)
        {
            Scene root = session.Root();
            string account = root.GetComponent<GateSessionKeyComponent>().GetAccount(request.GateTokenKey);
            if (account == null)
            {
                response.Error = ErrorCore.ERR_ConnectGateKeyError;
                response.Message = "Gate key验证失败!";
                session?.Disconnect().Coroutine();
                return;
            }
            
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
			if ( session.GetComponent<SessionLockingComponent>() != null)
			{
				response.Error = ErrorCode.ERR_RequestRepeatedly;
			//	reply();
				return;
			}
 
			root.GetComponent<GateSessionKeyComponent>().Remove(account);
			
			long instanceId = session.InstanceId;
			using (session.AddComponent<SessionLockingComponent>())
			using (await root.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.LoginGate, account.GetHashCode()))
			{
				if (instanceId != session.InstanceId)
				{
					return;
				}
				
				Log.Warning(">>>>>>>start login record");
				//通知登录中心服 记录本次登录的服务器Zone
				//1111  StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.LoginCenterConfig;
				StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.LoginCenterConfig;
				G2L_AddLoginRecord g2LAddLoginRecord = G2L_AddLoginRecord.Create();
				g2LAddLoginRecord.Account = account;
				g2LAddLoginRecord.ServerId = root.Zone();
				L2G_AddLoginRecord l2ARoleLogin = (L2G_AddLoginRecord) await root.GetComponent<MessageSender>().Call(loginCenterConfig.ActorId, g2LAddLoginRecord);

				Log.Warning(">>>>>>>end login record");

				if (l2ARoleLogin.Error != ErrorCode.ERR_Success)
				{
					response.Error = l2ARoleLogin.Error;
				//	reply();
					session?.Disconnect().Coroutine();
					return;
				}


				SessionStateComponent SessionStateComponent = session.GetComponent<SessionStateComponent>() ?? session.AddComponent<SessionStateComponent>();
				SessionStateComponent.State = SessionState.Normal;
			
				Player player =  root.GetComponent<PlayerComponent>().Get(account);
				if (player == null)
				{           
					// 添加一个新的GateUnit
					Log.Warning(">>>>>request.RoleId:"+request.RoleId);
					player = root.GetComponent<PlayerComponent>().AddChildWithId<Player,string,long>(request.RoleId,account,request.RoleId);
					player.PlayerState = PlayerState.Gate;
					root.GetComponent<PlayerComponent>().Add(player);
					
					PlayerSessionComponent playerSessionComponent = player.AddComponent<PlayerSessionComponent>();
					playerSessionComponent.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.GateSession);
					await playerSessionComponent.AddLocation(LocationType.GateSession);
					 
					player.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
					await player.AddLocation(LocationType.Player);
			
					session.AddComponent<SessionPlayerComponent>().Player = player;
					session.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.GateSession);
					playerSessionComponent.gateSession = session;
				}
				else
				{
					player.RemoveComponent<PlayerOfflineOutTimeComponent>();
				}
				
				// session.AddComponent<SessionPlayerComponent>().PlayerId = player.Id;
				// session.GetComponent<SessionPlayerComponent>().PlayerInstanceId = player.InstanceId;
				// session.GetComponent<SessionPlayerComponent>().Account = account;
				// player.ClientSession = session;
			}
			//reply();
            await ETTask.CompletedTask;
        }

       
        
    }
}