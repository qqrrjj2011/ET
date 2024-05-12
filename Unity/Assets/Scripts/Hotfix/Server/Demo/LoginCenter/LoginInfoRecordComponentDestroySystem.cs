namespace ET.Server
{
    [FriendOf(typeof(LoginInfoRecordComponent))]
    public class LoginInfoRecordComponentDestroySystem : DestroySystem<LoginInfoRecordComponent>
    {
        protected override void Destroy(LoginInfoRecordComponent self)
        {
            self.AccountLoginInfoDict.Clear();
        }
    }

    [FriendOf(typeof(LoginInfoRecordComponent))]
    public static class LoginInfoRecordComponentSystem
    {
        public static void Add(this LoginInfoRecordComponent self, string key, int value)
        {
            if (self.AccountLoginInfoDict.ContainsKey(key))
            {
                self.AccountLoginInfoDict[key] = value;
                return;
            }
            self.AccountLoginInfoDict.Add(key,value);
        }

        public static void Remove(this LoginInfoRecordComponent self, string key)
        {
            if (self.AccountLoginInfoDict.ContainsKey(key))
            {
                self.AccountLoginInfoDict.Remove(key);
            }
        }

        public static int Get(this LoginInfoRecordComponent self, string key)
        {
            if (!self.AccountLoginInfoDict.TryGetValue(key,out int value))
            {
                return -1;
            }
            
            return value;
        }

        public static bool IsExist(this LoginInfoRecordComponent self, string key)
        {
            return self.AccountLoginInfoDict.ContainsKey(key);
        }
    }
}

