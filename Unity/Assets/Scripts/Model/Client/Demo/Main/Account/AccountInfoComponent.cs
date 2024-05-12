namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class AccountInfoComponent:Entity,IAwake
    {
        public string Token;
       // public long AccountId;
        public string RealmKey;
        public string RealmAddress;
        //1111 用户名
        public string Account;
    }
}

