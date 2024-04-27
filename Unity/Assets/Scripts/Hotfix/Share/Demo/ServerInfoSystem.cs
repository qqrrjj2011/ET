namespace ET
{
    [FriendOf(typeof(ServerInfo))]
    public  static partial class ServerInfoSystem
    {
        public static void FromMessage(this ServerInfo self, ServerInfoProto serverInfoProto)
        {
            self.Status = serverInfoProto.Status;
            self.ServerName = serverInfoProto.ServerName;
        }

        public static ServerInfoProto ToMessage(this ServerInfo self)
        {
            ServerInfoProto serverInfoProto = ServerInfoProto.Create();
            serverInfoProto.Id = self.Id;
            serverInfoProto.ServerName = self.ServerName;
            serverInfoProto.Status = self.Status;
            return serverInfoProto;
        }

    }
    
}

