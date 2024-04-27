using System;

namespace ET.Client
{

    [FriendOf(typeof(RoleInfosComponent))]
    [FriendOf(typeof(AccountInfoComponent))]
    [FriendOf(typeof(ClientServerInfosComponent))]
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene scene, string account, string password)
        {
            scene.RemoveComponent<ClientSenderComponent>();
            
            ClientSenderComponent clientSenderComponent = scene.AddComponent<ClientSenderComponent>();
            
            NetClient2Main_Login response = await clientSenderComponent.LoginAsync(account, password);
            if (response.Error != ErrorCode.ERR_Success)
            {
                Log.Error(response.Error+"");
                return response.Error;
            }

            // 登录成功,保存accountinfo
            scene.GetComponent<AccountInfoComponent>().Token = response.Token;
            scene.GetComponent<AccountInfoComponent>().AccountId = response.AccountId;
            scene.GetComponent<AccountInfoComponent>().account = account;
            
            // 获得服务器列表
            int errorCode = await GetServerInfos(scene);
            if (errorCode != ErrorCode.ERR_Success)
            {
                Log.Error(errorCode.ToString());
            }
            
            return errorCode;
             
           //111 await EventSystem.Instance.PublishAsync(scene, new LoginFinish());
        }
        
        public static async ETTask<int> GetServerInfos(Scene scene)
        {
            A2C_GetServerInfos a2CGetServerInfos = null;
            try
            {
                C2A_GetServerInfos c2AGetServerInfos = C2A_GetServerInfos.Create();
                c2AGetServerInfos.AccountId = scene.GetComponent<AccountInfoComponent>().AccountId;
                c2AGetServerInfos.Token = scene.GetComponent<AccountInfoComponent>().Token;
                a2CGetServerInfos = await scene.GetComponent<ClientSenderComponent>().Call(c2AGetServerInfos) as A2C_GetServerInfos;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2CGetServerInfos.Error != ErrorCode.ERR_Success)
            {
                return a2CGetServerInfos.Error;
            }

            foreach (var serverInfoProto in a2CGetServerInfos.ServerInfosList)
            {
                ServerInfo serverInfo = scene.GetComponent<ClientServerInfosComponent>().AddChildWithId<ServerInfo>(serverInfoProto.Id);
                serverInfo.FromMessage(serverInfoProto);
                scene.GetComponent<ClientServerInfosComponent>().Add(serverInfo);
            }
           
            return ErrorCode.ERR_Success;
        }
        
          public static async ETTask<int> GetRoles(Scene zoneScene)
        {
            A2C_GetRoles a2CGetRoles = null;
            
            try
            {
                C2A_GetRoles c2AGetRoles = C2A_GetRoles.Create();
                c2AGetRoles.AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId;
                c2AGetRoles.Token = zoneScene.GetComponent<AccountInfoComponent>().Token;
                c2AGetRoles.ServerId = zoneScene.GetComponent<ClientServerInfosComponent>().CurrentServerId;
                a2CGetRoles = await zoneScene.GetComponent<ClientSenderComponent>().Call(c2AGetRoles) as A2C_GetRoles;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2CGetRoles.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2CGetRoles.Error.ToString());
                return a2CGetRoles.Error;
            }

        
            zoneScene.GetComponent<RoleInfosComponent>().RoleInfos.Clear();
            if (a2CGetRoles.RoleInfo != null)
            {
                foreach (var roleInfoProto in a2CGetRoles.RoleInfo)
                {
                    RoleInfo roleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
                    roleInfo.FromMessage(roleInfoProto);
                    zoneScene.GetComponent<RoleInfosComponent>().RoleInfos.Add(roleInfo);
                }
            }
            // else
            // {
            //     zoneScene.GetComponent<RoleInfosComponent>().RoleInfos = new List<RoleInfo>();
            // }

            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> CreateRole(Scene zoneScene, string name)
        {
            A2C_CreateRole a2CCreateRole = null;

            try
            {
                C2A_CreateRole c2ACreateRole = C2A_CreateRole.Create();
                c2ACreateRole.AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId;
                c2ACreateRole.Token = zoneScene.GetComponent<AccountInfoComponent>().Token;
                c2ACreateRole.Name = name;
                c2ACreateRole.ServerId = zoneScene.GetComponent<ClientServerInfosComponent>().CurrentServerId;
                a2CCreateRole = await zoneScene.GetComponent<ClientSenderComponent>().Call(c2ACreateRole) as A2C_CreateRole;

            }
            catch (Exception e)
            {
               Log.Error(e.ToString());
               return ErrorCode.ERR_NetWorkError;
            }

            if (a2CCreateRole.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2CCreateRole.Error.ToString());
                return a2CCreateRole.Error;
            }

            RoleInfo newRoleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChild<RoleInfo>();
            newRoleInfo.FromMessage(a2CCreateRole.RoleInfo);
            
            zoneScene.GetComponent<RoleInfosComponent>().RoleInfos.Add(newRoleInfo);

            return ErrorCode.ERR_Success;
        }
        
        public static async ETTask<int> DeleteRole(Scene zoneScene)
        {
            A2C_DeleteRole a2CDeleteRole = null;

            try
            {
                C2A_DeleteRole c2ADeleteRole = C2A_DeleteRole.Create();
                c2ADeleteRole.Token = zoneScene.GetComponent<AccountInfoComponent>().Token;
                c2ADeleteRole.AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId;
                c2ADeleteRole.RoleInfoId = zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId;
                c2ADeleteRole.ServerId = zoneScene.GetComponent<ClientServerInfosComponent>().CurrentServerId;
                a2CDeleteRole = await zoneScene.GetComponent<ClientSenderComponent>().Call(c2ADeleteRole) as A2C_DeleteRole;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2CDeleteRole.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2CDeleteRole.Error.ToString());
                return a2CDeleteRole.Error;
            }

            
            int index = zoneScene.GetComponent<RoleInfosComponent>().RoleInfos.FindIndex((refinfo) =>
            {
                RoleInfo info = refinfo;
                return info.Id == a2CDeleteRole.DeletedRoleInfoId;
            });
            
            zoneScene.GetComponent<RoleInfosComponent>().RoleInfos.RemoveAt(index);
            
            
            await ETTask.CompletedTask;
            return ErrorCode.ERR_Success;
        }


        public static async ETTask<int> GetRealmKey(Scene zoneScene)
        {
            A2C_GetRealmKey a2CGetRealmKey = null;

            try
            {
                C2A_GetRealmKey c2AGetRealmKey = C2A_GetRealmKey.Create();
                c2AGetRealmKey.Token = zoneScene.GetComponent<AccountInfoComponent>().Token;
                c2AGetRealmKey.AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId;
                c2AGetRealmKey.ServerId = zoneScene.GetComponent<ClientServerInfosComponent>().CurrentServerId;
                a2CGetRealmKey = await zoneScene.GetComponent<ClientSenderComponent>().Call(c2AGetRealmKey) as A2C_GetRealmKey;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2CGetRealmKey.Error != ErrorCode.ERR_Success)
            {
                Log.Error(a2CGetRealmKey.Error.ToString());
                return a2CGetRealmKey.Error;
            }

            zoneScene.GetComponent<AccountInfoComponent>().RealmKey = a2CGetRealmKey.RealmKey;
            zoneScene.GetComponent<AccountInfoComponent>().RealmAddress = a2CGetRealmKey.RealmAddress;
            zoneScene.GetComponent<SessionComponent>().Session.Dispose();
            await ETTask.CompletedTask;
            return ErrorCode.ERR_Success;
        }
        
         public static async ETTask<int> EnterGame(Scene zoneScene)
        {
            // 1. 连接Realm，获取分配的Gate
           //  R2C_LoginRealm r2CLogin;
           // IPEndPoint realmAddress = NetworkHelper.ToIPEndPoint(zoneScene.GetComponent<AccountInfoComponent>().RealmAddress);
           //  Session session =  await RouterHelper.CreateRouterSession(zoneScene, realmAddress);
           //  try
           //  {
           //      
           //      r2CLogin = (R2C_LoginRealm) await session.Call(new C2R_LoginRealm() 
           //                                      {    
           //                                          AccountId =  zoneScene.GetComponent<AccountInfoComponent>().AccountId,
           //                                          RealmTokenKey = zoneScene.GetComponent<AccountInfoComponent>().RealmKey 
           //                                      });
           //  }
           //  catch (Exception e)
           //  {
           //      Log.Error(e);
           //      session?.Dispose();
           //      return ErrorCode.ERR_NetWorkError;
           //  }
           //  session?.Dispose();
           //
           //
           //  if (r2CLogin.Error != ErrorCode.ERR_Success)
           //  {
           //      return r2CLogin.Error;
           //  }
           //  
           //  Log.Warning($"GateAddress :  {r2CLogin.GateAddress}");
           // Session gateSession = await RouterHelper.CreateRouterSession(zoneScene, NetworkHelper.ToIPEndPoint(r2CLogin.GateAddress));
           //  zoneScene.GetComponent<SessionComponent>().Session = gateSession;
           //  
           //  // 2. 开始连接Gate
           //  long currentRoleId = zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId;
           //  G2C_LoginGameGate g2CLoginGate = null;
           //  try
           //  {
           //      long accountId = zoneScene.GetComponent<AccountInfoComponent>().AccountId;
           //       g2CLoginGate = (G2C_LoginGameGate)await gateSession.Call(new C2G_LoginGameGate() { Key = r2CLogin.GateSessionKey, Account = accountId,RoleId = currentRoleId});
           //      
           //  }
           //  catch (Exception e)
           //  {
           //      Log.Error(e);
           //      zoneScene.GetComponent<SessionComponent>().Session.Dispose();
           //      return ErrorCode.ERR_NetWorkError;
           //  }
           //  
           //  if (g2CLoginGate.Error != ErrorCode.ERR_Success)
           //  {
           //      zoneScene.GetComponent<SessionComponent>().Session.Dispose();
           //      return g2CLoginGate.Error;
           //  }
           //  Log.Debug(">>>>登陆gate成功!");
           //
           //  //3. 角色正式请求进入游戏逻辑服
           //  G2C_EnterGame g2CEnterGame = null;
           //  try
           //  {
           //      g2CEnterGame = (G2C_EnterGame)await gateSession.Call(new C2G_EnterGame() { });
           //  }
           //  catch (Exception e)
           //  {
           //      Log.Error(e);
           //      zoneScene.GetComponent<SessionComponent>().Session.Dispose();
           //      return ErrorCode.ERR_NetWorkError;
           //  }
           //
           //  if (g2CEnterGame.Error != ErrorCode.ERR_Success)
           //  {
           //      Log.Error(g2CEnterGame.Error.ToString());
           //      return g2CEnterGame.Error;
           //  }
           //  
           //  Log.Debug("角色进入游戏成功!!!!");
           //  zoneScene.GetComponent<PlayerComponent>().MyId = g2CEnterGame.MyId;
           //  
           //  await zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_SceneChangeFinish>();
           //  await EventSystem.Instance.PublishAsync(zoneScene, new ET.EventType.SceneChangeFinish());
            return ErrorCode.ERR_Success;
        }
        
         
    }
}