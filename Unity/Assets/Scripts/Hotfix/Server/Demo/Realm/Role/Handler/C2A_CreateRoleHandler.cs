using System;

namespace ET.Server
{
    [FriendOf(typeof(ServerRoleInfo))]
    [MessageSessionHandler(SceneType.Realm)]
    public class C2A_CreateRoleHandler : MessageSessionHandler<C2A_CreateRole,A2C_CreateRole>
    {
        protected override async ETTask Run(Session session, C2A_CreateRole request, A2C_CreateRole response)
        {
            // if (session.DomainScene().SceneType != SceneType.Account)
            // {
            //     Log.Error($"请求的Scene错误，当前Scene为：{session.DomainScene().SceneType}");
            //     session.Dispose();
            //     return;
            // }
            
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_LoginRepeatedly;
              //  reply();
                session.Disconnect().Coroutine();
                return;
            }
            
            string token = session.Root().GetComponent<TokenComponent>().Get(request.Account);

            if (token == null || token != request.Token)
            {
                response.Error = ErrorCode.ERR_TokenError;
              //  reply();
                session?.Disconnect().Coroutine();
                return;
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                response.Error = ErrorCode.ERR_RoleNameEmpty;
              //  reply();
                return;
            }

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await session.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.CreateRole,request.Account.GetHashCode()))
                {
                    var roleInfos = await session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone())
                            .Query<ServerRoleInfo>(d => d.Name == request.Name && d.ServerId == request.ServerId && d.State != (int)RoleInfoState.Freeze);

                    // 重名
                    if (roleInfos != null && roleInfos.Count > 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNameRepeat;
                     // reply();
                        return;
                    }

                    long roleID = IdGenerater.Instance.GenerateUnitId(request.ServerId);
                    ServerRoleInfo newServerRoleInfo = session.GetComponent<RoleInfosZone>().AddChildWithId<ServerRoleInfo>(roleID);
                    newServerRoleInfo.Name = request.Name;
                    newServerRoleInfo.State = (int)RoleInfoState.Normal;
                    newServerRoleInfo.ServerId = request.ServerId;
                    newServerRoleInfo.Account = request.Account;
                    newServerRoleInfo.CreateTime = TimeInfo.Instance.ServerNow();
                    newServerRoleInfo.LastLoginTime = 0;
                    
                    Log.Warning(">>>>>>>>>newServerRoleInfo:"+newServerRoleInfo.Id);

                    await session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone()).Save<ServerRoleInfo>(newServerRoleInfo);

                    response.RoleInfo = newServerRoleInfo.ToMessage();
                    
                    // reply();

                    newServerRoleInfo?.Dispose();
                }
            }
           
        }
    }
}