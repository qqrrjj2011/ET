using System.Text.RegularExpressions;
using NativeCollection;

namespace ET.Server
{
    [FriendOf(typeof(AccountInfo))]
    [MessageSessionHandler(SceneType.Realm)]
    public class C2R_LoginAccountHandler:MessageSessionHandler<C2A_LoginAccount,A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response)
        {
            Log.Warning(">>>>>>>>>> account:"+request.Account + " password:"+request.Password);
            
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            // 防止前端多次发送，或者网络攻击
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_LoginRepeatedly;
                session.Disconnect().Coroutine();
            }


            if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginInfoNullOrEmpty;
                session.Disconnect().Coroutine();
                return;
            }
            
            // 验证用户名格式
            // if (!Regex.IsMatch(request.Account.Trim(), @"^(?=.* [0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            // {
            //     response.Error = ErrorCode.ERR_AccountNameFormError;
            //     session.Disconnect().Coroutine();
            //     return;
            // }
            //
            // // 验证密码格式
            // if (!Regex.IsMatch(request.Password.Trim(), @"^[A-Za-z0-9]+$"))
            // {
            //     response.Error = ErrorCode.ERR_PassWordFormError;
            //     session.Disconnect().Coroutine();
            //     return;
            // }
            
            if (session.GetComponent<RoleInfosZone>() == null)
            {
                session.AddComponent<RoleInfosZone>();
            }

            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();
            using (session.AddComponent<SessionLockingComponent>())
            {
                // 线程锁，防止两台电脑相同账号同时登录
                using (await coroutineLockComponent.Wait(CoroutineLockType.LoginAccount, request.Account.GetHashCode()))
                {
                    DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());
                    var accounts = await dbComponent.Query<AccountInfo>(x => x.Account.Equals(request.Account));
                    AccountInfo accountInfo = null;
                    if (accounts != null && accounts.Count > 0)
                    {
                        accountInfo = accounts[0];
                        // 安全起见，加入session
                        session.AddChild(accountInfo);

                        if (accountInfo.AccountType == (int)AccountType.Black)
                        {
                            response.Error = ErrorCode.ERR_BlackAccount;
                            session.Disconnect().Coroutine();
                            accountInfo.Dispose();
                            return;
                        }

                        if (!accountInfo.PassWord.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_PassWordFormError;
                            session.Disconnect().Coroutine();
                            accountInfo.Dispose();
                            return;
                        }
                    }
                    // 创建新账户
                    else
                    {
                        accountInfo = session.AddChild<AccountInfo>();
                        accountInfo.PassWord = request.Password;
                        accountInfo.Account = request.Account;
                        accountInfo.AccountType = (int)AccountType.General;
                        accountInfo.CreateTime = TimeInfo.Instance.ServerNow();
                        await dbComponent.Save<AccountInfo>(accountInfo);
                    }
                    
                    StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.Zone(), "LoginCenter");
                    A2L_LoginAccountRequest a2LLoginAccountRequest = A2L_LoginAccountRequest.Create();
                    a2LLoginAccountRequest.Account = accountInfo.Account;
                    var loginAccountResponse  = (L2A_LoginAccountResponse) await session.Root().GetComponent<MessageSender>().Call(startSceneConfig.ActorId,a2LLoginAccountRequest);
                    if (loginAccountResponse.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = loginAccountResponse.Error;
                        session?.Disconnect().Coroutine();
                        accountInfo?.Dispose();
                        return; 
                    }
                    
                    Session otherSession = session.Root().GetComponent<AccountSessionsComponent>().Get(accountInfo.Account);
                    A2C_Disconnect a2CDisconnect = A2C_Disconnect.Create();
                    a2CDisconnect.Error = 0;
                    otherSession?.Send(a2CDisconnect);
                    otherSession?.Disconnect().Coroutine();
                    
                    session.Root().GetComponent<AccountSessionsComponent>().Add(accountInfo.Account,session);
                    session.AddComponent<AccountCheckOutTimeComponent, string>(accountInfo.Account);
                    
                    string Token = TimeInfo.Instance.ServerNow().ToString() + RandomGenerator.RandomNumber(int.MinValue, int.MaxValue);
                    session.Root().GetComponent<TokenComponent>().Remove(accountInfo.Account);
                    session.Root().GetComponent<TokenComponent>().Add(accountInfo.Account,Token);
                    
                  //  response.AccountId = accountInfo.Id;
                    response.Token     = Token;

                  //  Log(">>>>>>>>>AccountID:"+accountInfo.Id);
                    
                    accountInfo?.Dispose();
                    
                    
                }
            }
             
            await ETTask.CompletedTask;
        }
    }
}

