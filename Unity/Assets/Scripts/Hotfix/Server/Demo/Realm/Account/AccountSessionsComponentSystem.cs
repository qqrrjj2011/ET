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
        public static Session Get(this AccountSessionsComponent self, long accountId)
        {
            if (!self.AccountSessionDictionary.TryGetValue(accountId,out EntityRef<Session> session))
            {
                return null;
            }

            return session;
        }

        public static void Add(this AccountSessionsComponent self, long accountId, Session session)
        {
            if (self.AccountSessionDictionary.ContainsKey(accountId))
            {
                self.AccountSessionDictionary[accountId] = session;
                return;
            }
            self.AccountSessionDictionary.Add(accountId,session);
        }


        public static void Remove(this AccountSessionsComponent self, long accountId)
        {
            if (self.AccountSessionDictionary.ContainsKey(accountId))
            {
                self.AccountSessionDictionary.Remove(accountId);
            }
        }

    }
    
}

