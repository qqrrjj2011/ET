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
            scene.GetComponent<AccountInfoComponent>().Account = response.Account;
            scene.GetComponent<AccountInfoComponent>().Account = account;
            
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
                c2AGetServerInfos.Account = scene.GetComponent<AccountInfoComponent>().Account;
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
                c2AGetRoles.Account = zoneScene.GetComponent<AccountInfoComponent>().Account;
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
                    RoleInfo roleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChildWithId<RoleInfo>(roleInfoProto.Id);
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
                c2ACreateRole.Account = zoneScene.GetComponent<AccountInfoComponent>().Account;
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

            RoleInfo newRoleInfo = zoneScene.GetComponent<RoleInfosComponent>().AddChildWithId<RoleInfo>(a2CCreateRole.RoleInfo.Id);
            newRoleInfo.FromMessage(a2CCreateRole.RoleInfo);
//            Log.Error("client roleId:"+newRoleInfo.Id);
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
                c2ADeleteRole.Account = zoneScene.GetComponent<AccountInfoComponent>().Account;
                c2ADeleteRole.RoleId = zoneScene.GetComponent<RoleInfosComponent>().CurrentRoleId;
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
                return info.Id == a2CDeleteRole.DeletedRoleId;
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
                c2AGetRealmKey.Account = zoneScene.GetComponent<AccountInfoComponent>().Account;
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
            // 获取网关gate 地址和 totken
            R2C_GetGateKey r2CGetGateKey;
            try
            {
                C2R_GetGateKey c2RGetGateKey = C2R_GetGateKey.Create();
                c2RGetGateKey.RealmTokenKey = zoneScene.GetComponent<AccountInfoComponent>().Token;
                c2RGetGateKey.Account = zoneScene.GetComponent<AccountInfoComponent>().Account;
                c2RGetGateKey.ServerId = zoneScene.GetComponent<ClientServerInfosComponent>().CurrentServerId;
                r2CGetGateKey = await zoneScene.GetComponent<ClientSenderComponent>().Call(c2RGetGateKey) as R2C_GetGateKey;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (r2CGetGateKey.Error != ErrorCode.ERR_Success)
            {
                return r2CGetGateKey.Error;
            }
            
            
            // 登录网关
            NetClient2Main_LoginGate netClient2MainLoginGate = await zoneScene.GetComponent<ClientSenderComponent>().LoginGateAsync(zoneScene.GetComponent<AccountInfoComponent>().Account,r2CGetGateKey.GateSessionTokey,r2CGetGateKey.GateAddress);
            if (netClient2MainLoginGate.Error != ErrorCode.ERR_Success)
            {
                return netClient2MainLoginGate.Error;
            }
            
            Log.Info(">>>>>>>登录网关成功");


           //3. 角色正式请求进入游戏逻辑服
           G2C_EnterGame g2CEnterGame = null;
           try
           {
               C2G_EnterGame c2GEnterGame = C2G_EnterGame.Create();
               g2CEnterGame = (G2C_EnterGame)await zoneScene.GetComponent<ClientSenderComponent>().Call(c2GEnterGame);
           }
           catch (Exception e)
           {
               Log.Error(e);
               zoneScene.GetComponent<SessionComponent>().Session.Dispose();
               return ErrorCode.ERR_NetWorkError;
           }
           
           if (g2CEnterGame.Error != ErrorCode.ERR_Success)
           {
               Log.Error(g2CEnterGame.Error.ToString());
               return g2CEnterGame.Error;
           }
           
           Log.Debug("角色进入游戏成功!!!!");
           zoneScene.GetComponent<PlayerComponent>().MyId = g2CEnterGame.MyId;
           
           await zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_SceneChangeFinish>();
           await EventSystem.Instance.PublishAsync(zoneScene, new EventType.SceneChangeFinish());
           
           return ErrorCode.ERR_Success;
           
        }
        
         
    }
}