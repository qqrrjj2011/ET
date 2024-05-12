using System;
using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(ServerRoleInfo))]
    [MessageSessionHandler(SceneType.Realm)]
    public class C2A_GetRolesHandler : MessageSessionHandler<C2A_GetRoles,A2C_GetRoles>
    {
        protected override async ETTask Run(Session session, C2A_GetRoles request, A2C_GetRoles response)
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
             //   reply();
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
            
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await session.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.CreateRole,request.Account.GetHashCode()))
                {
                    var roleInfos = await session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone())
                            .Query<ServerRoleInfo>(d => d.Account == request.Account && d.ServerId == request.ServerId && d.State == (int)RoleInfoState.Normal);

                    if (roleInfos == null || roleInfos.Count == 0)
                    {
                      //  reply();
                        return;
                    }

                    if (response.RoleInfo == null) response.RoleInfo = new List<RoleInfoProto>();
                    foreach (var roleInfo in roleInfos)
                    {
                        response.RoleInfo.Add(roleInfo.ToMessage()); 
                        Log.Warning(">>>>>>>>>roleID:"+roleInfo.Id);
                        roleInfo?.Dispose();
                    }
                    roleInfos.Clear();
                    
                  //  reply();
                }
            }
            
            
            
        }
    }
}