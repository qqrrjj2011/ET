namespace ET.Client
{
    [FriendOf(typeof(ServerInfo))]
    public static class ServerInfoSystem
    {
        public static void FromMessage(this ServerInfo self, ServerInfoProto serverInfoProto)
        {
            self.Status = (ServerStatus)serverInfoProto.Status;
            self.ServerName = serverInfoProto.ServerName;
        }

        public static ServerInfoProto ToMessage(this ServerInfo self)
        {
            ServerInfoProto serverInfoProto = ServerInfoProto.Create();
            serverInfoProto.ServerName = self.ServerName;
            serverInfoProto.Status = (int)self.Status;
            return serverInfoProto;
        }
        
    }
}