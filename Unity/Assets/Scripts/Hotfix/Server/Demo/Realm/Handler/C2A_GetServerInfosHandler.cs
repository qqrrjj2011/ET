using System.Runtime.InteropServices;

namespace ET.Server
{
    [FriendOf(typeof(ServerInfosComponent))]
    [MessageSessionHandler(SceneType.Realm)]
    public class C2A_GetServerInfosHandler:MessageSessionHandler<C2A_GetServerInfos,A2C_GetServerInfos>
    {
        protected override async ETTask Run(Session session, C2A_GetServerInfos request, A2C_GetServerInfos response)
        {
            string curToken = session.Root().GetComponent<TokenComponent>().Get(request.Account);
            if (!curToken.Equals(request.Token))
            {
                response.Error = ErrorCode.ERR_TokenError;
                session.Disconnect().Coroutine();
                return;
            }

            foreach (var VARIABLE in session.Root().GetComponent<ServerInfosComponent>().ServerInfoList)
            {
                ServerInfo info = VARIABLE;
                response.ServerInfosList.Add(info.ToMessage());
            }
            
            await ETTask.CompletedTask;
        }
    }
}
