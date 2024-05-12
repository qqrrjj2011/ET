namespace ET.Server
{
    [FriendOf(typeof(TokenComponent))]
    public static class TokenComponentSystem
    {
        public static void Add(this TokenComponent self, string account, string token)
        {
            self.TokenDictionary.Add(account,token);
            self.TimeOutRemoveKey(account,token).Coroutine();
        }

        public static string Get(this TokenComponent self, string account)
        {
            string value = null;
            self.TokenDictionary.TryGetValue(account, out value);
            return value;
        }

        public static void Remove(this TokenComponent self, string account)
        {
            if (self.TokenDictionary.ContainsKey(account))
            {
                self.TokenDictionary.Remove(account);
            }
        }

        private static async ETTask TimeOutRemoveKey(this TokenComponent self, string account, string tokenKey)
        {
            await self.Root().GetComponent<TimerComponent>().WaitAsync(600000);

            string onlineToken = self.Get(account);

            if (!string.IsNullOrEmpty(onlineToken) && onlineToken == tokenKey)
            {
                self.Remove(account);
            }

        }
        
        
    }
    
}

 