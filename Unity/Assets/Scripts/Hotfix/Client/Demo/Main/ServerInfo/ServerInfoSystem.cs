namespace ET.Client
{
    [FriendOf(typeof(ServerInfo))]
    public static class ServerInfoSystem
    {
        public static void FromMessage(this ServerInfo self, ServerInfoProto serverInfoProto)
        {
            self.ServerInfoId = serverInfoProto.Id;
            self.Status = serverInfoProto.Status;
            self.ServerName = serverInfoProto.ServerName;
        }

        public static ServerInfoProto ToMessage(this ServerInfo self)
        {
            ServerInfoProto serverInfoProto = ServerInfoProto.Create();
            serverInfoProto.Id = self.ServerInfoId;
            serverInfoProto.ServerName = self.ServerName;
            serverInfoProto.Status = self.Status;
            return serverInfoProto;
        }
        
    }
}