using System;

namespace ET.Server
{
    [FriendOf(typeof(ServerRoleInfo))]
    [MessageSessionHandler(SceneType.Realm)]
    public class C2A_DeleteRoleHandler : MessageSessionHandler<C2A_DeleteRole,A2C_DeleteRole>
    {
        protected override async ETTask Run(Session session, C2A_DeleteRole request, A2C_DeleteRole response)
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
            //    reply();
                session?.Disconnect().Coroutine();
                return;
            }

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await session.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.CreateRole, request.Account.GetHashCode()))
                {
                    var roleInfos = await session.Root().GetComponent<DBManagerComponent>().GetZoneDB(request.ServerId)
                            .Query<ServerRoleInfo>(d => d.Id == request.RoleId && d.ServerId == request.ServerId);

                    if (roleInfos == null || roleInfos.Count <= 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNotExist;
                    //    reply();
                        return;
                    }

                    var roleInfo = roleInfos[0];
                    session.GetComponent<RoleInfosZone>().AddChild(roleInfo);
                    roleInfo.State = (int)RoleInfoState.Freeze;
                    
                    await session.Root().GetComponent<DBManagerComponent>().GetZoneDB(request.ServerId).Save(roleInfo);
                    response.DeletedRoleId = roleInfo.Id;
                    roleInfo?.Dispose();

                 //   reply();
                }
            }

            await ETTask.CompletedTask;
        }
    }
}