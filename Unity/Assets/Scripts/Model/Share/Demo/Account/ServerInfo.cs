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
        public ServerStatus Status;
        public string ServerName;
    }
}