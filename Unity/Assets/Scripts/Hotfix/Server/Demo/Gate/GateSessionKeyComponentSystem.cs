namespace ET.Server
{
    [FriendOf(typeof(GateSessionKeyComponent))]
    public static partial class GateSessionKeyComponentSystem
    {
        public static void Add(this GateSessionKeyComponent self,  string account, string key)
        {
            self.sessionKey.Add(account, key);
            self.TimeoutRemoveKey(account).Coroutine();
        }

        public static string Get(this GateSessionKeyComponent self, string account)
        {
            string key = null;
            self.sessionKey.TryGetValue(account, out key);
            return key;
        }
        
        public static string GetAccount(this GateSessionKeyComponent self, string key)
        {
            foreach (var VARIABLE in self.sessionKey)
            {
                if (VARIABLE.Value == key)
                {
                    return VARIABLE.Key;
                }
            }

            return null;
        }

        public static void Remove(this GateSessionKeyComponent self, string account)
        {
            self.sessionKey.Remove(account);
        }

        private static async ETTask TimeoutRemoveKey(this GateSessionKeyComponent self, string account)
        {
            await self.Root().GetComponent<TimerComponent>().WaitAsync(20000);
            self.sessionKey.Remove(account);
        }
    }
}