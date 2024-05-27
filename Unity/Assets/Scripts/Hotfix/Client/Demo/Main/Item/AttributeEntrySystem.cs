namespace ET.Client
{
    [FriendOf(typeof(AttributeEntry))]
    public class AttributeEntryDestorySystem: DestroySystem<AttributeEntry>
    {
        protected override void Destroy(AttributeEntry self)
        {
            self.Key   = 0;
            self.Value = 0;
            self.Type  = EntryType.Common;
        }
    }
    
    [FriendOf(typeof(AttributeEntry))]
    public static class AttributeEntrySystem
    {
        public static void FromMessage(this AttributeEntry self, AttributeEntryProto attributeEntryProto)
        {
          //  self.Id = attributeEntryProto.Id;
            self.Key = attributeEntryProto.Key;
            self.Value = attributeEntryProto.Value;
            self.Type = (EntryType)attributeEntryProto.EntryType;
        }
    }
}