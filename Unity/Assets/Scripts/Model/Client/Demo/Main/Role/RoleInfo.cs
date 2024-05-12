using MongoDB.Bson.Serialization.Attributes;

namespace ET.Client
{
    public enum RoleInfoState
    {
        Normal = 0,
        Freeze,
    }
    
   // [ComponentOf]
    [ChildOf(typeof(RoleInfosComponent))]
    [BsonDiscriminator("Client.RoleInfo")]
    public class RoleInfo : Entity,IAwake
    {
        public string Name;

        public int ServerId;

        public int State;

        public string Account;

        public long LastLoginTime;

        public long CreateTime;
    }
}