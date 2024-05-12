using System;

namespace ET.Server
{
    [Invoke(TimerInvokeType.AccountSessionCheckOutTime)]
    public class  AccountSessionCheckOutTimer : ATimer<AccountCheckOutTimeComponent>
    {
        protected override void Run(AccountCheckOutTimeComponent self)
        {
            try
            {
                self.DeleteSession();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }

    [FriendOf(typeof(AccountCheckOutTimeComponent))]
    public class AccountCheckOutTimeComponentAwakeSystem: AwakeSystem<AccountCheckOutTimeComponent,string>
    {
        protected override void Awake(AccountCheckOutTimeComponent self, string account)
        {
            self.Account = account;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
            self.Timer = self.Root().GetComponent<TimerComponent>().NewOnceTimer(TimeInfo.Instance.ServerNow() + 600000, TimerInvokeType.AccountSessionCheckOutTime, self);
        }
    }
    
    [FriendOf(typeof(AccountCheckOutTimeComponent))]
    public class AccountCheckOutTimeComponentDestroySystem : DestroySystem<AccountCheckOutTimeComponent>
    {
        protected override void Destroy(AccountCheckOutTimeComponent self)
        {
            self.Account = "";
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }
    }
    [FriendOf(typeof(AccountCheckOutTimeComponent))]
    public static class AccountCheckOutTimeComponentSystem
    {

        public static void DeleteSession(this AccountCheckOutTimeComponent self)
        {
            Session session = self.GetParent<Session>();

            Session otherSession = session.Root().GetComponent<AccountSessionsComponent>().Get(self.Account);
            if (session.InstanceId == otherSession.InstanceId)
            {
                session.Root().GetComponent<AccountSessionsComponent>().Remove(self.Account);
            }
             
            A2C_Disconnect a2CDisconnect = A2C_Disconnect.Create();
            a2CDisconnect.Error = 1;
            session?.Send(a2CDisconnect);
            session?.Disconnect().Coroutine();
        }

    }
}

