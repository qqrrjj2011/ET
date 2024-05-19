
namespace ET.Client
{
    [FriendOf(typeof(EquipInfoComponent))]
    public class EquipInfoComponentDestorySystem: DestroySystem<EquipInfoComponent>
    {
        protected override void Destroy(EquipInfoComponent self)
        {
            self.IsInited  = false;
            self.Score     = 0;
            for (int i = 0; i < self.EntryList.Count; i++)
            {
                AttributeEntry ent = self.EntryList[i];
                ent?.Dispose();
            }
            self.EntryList.Clear();
        }
    }

    
    [FriendOf(typeof(AttributeEntry))]
    [FriendOf(typeof(EquipInfoComponent))]
    public static class EquipInfoComponentSystem
    {
        public static void FromMessage(this EquipInfoComponent self,EquipInfoProto equipInfoProto)
        {
            self.Score = equipInfoProto.Score;

            for (int i = 0; i < self.EntryList.Count; i++)
            {
                AttributeEntry ent = self.EntryList[i];
                ent?.Dispose();
            }
            self.EntryList.Clear();
            
            for (int i = 0; i < equipInfoProto.AttributeEntryProtoList.Count; i++)
            {
                AttributeEntry attributeEntry = self.AddChild<AttributeEntry>();
                attributeEntry.FromMessage(equipInfoProto.AttributeEntryProtoList[i]);
                self.EntryList.Add(attributeEntry);
            }

            self.IsInited = true;
        }
    }

}