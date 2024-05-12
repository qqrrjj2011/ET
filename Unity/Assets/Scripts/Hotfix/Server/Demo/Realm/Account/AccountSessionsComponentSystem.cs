namespace ET.Server
{
    [FriendOf(typeof(AccountSessionsComponent))]
    public class AccountSessionsComponentDestroySystem:DestroySystem<AccountSessionsComponent>
    {
        protected override void Destroy(AccountSessionsComponent self)
        {
            self.AccountSessionDictionary.Clear();
        }
    }

    [FriendOf(typeof(AccountSessionsComponent))]
    public static class AccountSessionsComponentSystem
    {
        public static Session Get(this AccountSessionsComponent self, string account)
        {
            if (!self.AccountSessionDictionary.TryGetValue(account,out EntityRef<Session> session))
            {
                return null;
            }

            return session;
        }

        public static void Add(this AccountSessionsComponent self, string account, Session session)
        {
            if (self.AccountSessionDictionary.ContainsKey(account))
            {
                self.AccountSessionDictionary[account] = session;
                return;
            }
            self.AccountSessionDictionary.Add(account,session);
        }


        public static void Remove(this AccountSessionsComponent self, string account)
        {
            if (self.AccountSessionDictionary.ContainsKey(account))
            {
                self.AccountSessionDictionary.Remove(account);
            }
        }

    }
    
}

