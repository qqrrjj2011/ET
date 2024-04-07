using System;

namespace ET.Client
{
    [FriendOf(typeof(AccountInfoComponent))]
    public static class LoginHelper
    {
        public static async ETTask Login(Scene scene, string account, string password)
        {
            scene.RemoveComponent<ClientSenderComponent>();
            
            ClientSenderComponent clientSenderComponent = scene.AddComponent<ClientSenderComponent>();
            
            NetClient2Main_Login response = await clientSenderComponent.LoginAsync(account, password);
            if (response.Error != ErrorCode.ERR_Success)
            {
                Log.Error(response.Error+"");
                return;
            }

            // 登录成功,保存accountinfo
            scene.GetComponent<AccountInfoComponent>().Token = response.Token;
            scene.GetComponent<AccountInfoComponent>().AccountId = response.AccountId;
            scene.GetComponent<AccountInfoComponent>().account = account;
            
            // 获得服务器列表
            int errorCode = await LoginHelper.GetServerInfos(scene);
            if (errorCode != ErrorCode.ERR_Success)
            {
                Log.Error(errorCode.ToString());
                return;
            }
             
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
                ServerInfo serverInfo = scene.GetComponent<ServerInfosComponent>().AddChild<ServerInfo>();
                serverInfo.FromMessage(serverInfoProto);
                scene.GetComponent<ServerInfosComponent>().Add(serverInfo);
            }
           
            return ErrorCode.ERR_Success;
        }

        
    }
}