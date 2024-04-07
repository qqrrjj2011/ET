namespace ET
{
    public enum ServerStatus
    {
        Normal = 0,
        Stop   = 1,
    }
    [ChildOf]
    public class ServerInfo : Entity,IAwake
    {
        public long ServerInfoId;
        public int Status;
        public string ServerName;
    }
}