namespace ET.Server
{
    [FriendOf(typeof(RoleInfo))]
    public static class RoleInfoSystem
    {
        public static void FromMessage(this RoleInfo self, RoleInfoProto roleInfoProto)
        {
           // self.Id = roleInfoProto.Id;
            self.Name = roleInfoProto.Name;
            self.State = roleInfoProto.State;
            self.AccountId = roleInfoProto.AccountId;
            self.CreateTime = roleInfoProto.CreateTime;
            self.ServerId = roleInfoProto.ServerId;
            self.LastLoginTime = roleInfoProto.LastLoginTime;
        }

        public static RoleInfoProto ToMessage(this RoleInfo self)
        {
            RoleInfoProto infoProto = RoleInfoProto.Create();
            infoProto.Id = self.Id;
            infoProto.Name = self.Name;
            infoProto.State = self.State;
            infoProto.AccountId = self.AccountId;
            infoProto.CreateTime = self.CreateTime;
            infoProto.ServerId = self.ServerId;
            infoProto.LastLoginTime = self.LastLoginTime;
            return infoProto;
        }
    }
}