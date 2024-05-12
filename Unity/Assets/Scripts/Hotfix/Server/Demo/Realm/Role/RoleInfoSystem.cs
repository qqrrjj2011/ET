namespace ET.Server
{
    [FriendOf(typeof(ServerRoleInfo))]
    public static class RoleInfoSystem
    {
        public static void FromMessage(this ServerRoleInfo self, RoleInfoProto roleInfoProto)
        {
            //self.Id = roleInfoProto.Id;
            self.Name = roleInfoProto.Name;
            self.State = roleInfoProto.State;
            self.Account = roleInfoProto.Account;
            self.CreateTime = roleInfoProto.CreateTime;
            self.ServerId = roleInfoProto.ServerId;
            self.LastLoginTime = roleInfoProto.LastLoginTime;
        }

        public static RoleInfoProto ToMessage(this ServerRoleInfo self)
        {
            RoleInfoProto infoProto = RoleInfoProto.Create();
            infoProto.Id = self.Id;
            infoProto.Name = self.Name;
            infoProto.State = self.State;
            infoProto.Account = self.Account;
            infoProto.CreateTime = self.CreateTime;
            infoProto.ServerId = self.ServerId;
            infoProto.LastLoginTime = self.LastLoginTime;
            return infoProto;
        }
    }
}