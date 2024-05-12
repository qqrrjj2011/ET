namespace ET.Server
{
    public enum RoleInfoState
    {
        Normal = 0,
        Freeze,
    }
    
    [ChildOf(typeof(RoleInfosZone))]
    public class ServerRoleInfo : Entity,IAwake
    {
        public string Name;

        public int ServerId;

        public int State;

        public string Account;

        public long LastLoginTime;

        public long CreateTime;
    }
}