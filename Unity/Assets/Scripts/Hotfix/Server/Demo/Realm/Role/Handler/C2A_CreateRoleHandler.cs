using System;

namespace ET.Server
{
    [FriendOf(typeof(RoleInfo))]
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
            
            string token = session.Root().GetComponent<TokenComponent>().Get(request.AccountId);

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
                using (await session.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.CreateRole,request.AccountId))
                {
                    var roleInfos = await session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone())
                            .Query<RoleInfo>(d => d.Name == request.Name && d.ServerId == request.ServerId && d.State != (int)RoleInfoState.Freeze);

                    if (roleInfos != null && roleInfos.Count > 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNameEmpty;
                     //   reply();
                        return;
                    }

                    RoleInfo newRoleInfo = session.GetComponent<RoleInfosZone>().AddChildWithId<RoleInfo>(IdGenerater.Instance.GenerateUnitId(request.ServerId));
                    newRoleInfo.Name = request.Name;
                    newRoleInfo.State = (int)RoleInfoState.Normal;
                    newRoleInfo.ServerId = request.ServerId;
                    newRoleInfo.AccountId = request.AccountId;
                    newRoleInfo.CreateTime = TimeInfo.Instance.ServerNow();
                    newRoleInfo.LastLoginTime = 0;

                    await session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone()).Save<RoleInfo>(newRoleInfo);

                    response.RoleInfo = newRoleInfo.ToMessage();

                  //  reply();

                    newRoleInfo?.Dispose();

                }
            }
           
        }
    }
}