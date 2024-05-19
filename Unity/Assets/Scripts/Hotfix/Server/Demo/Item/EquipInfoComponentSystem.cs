namespace ET.Server
{
    public class EquipInfoComponentAwakeSystem : AwakeSystem<EquipInfoComponent>
    {
        protected override void Awake(EquipInfoComponent self)
        {
            self.GenerateEntries();
        }
    }
    
    [FriendOf(typeof(EquipInfoComponent))]
    public class EquipInfoComponentDestorySystem: DestroySystem<EquipInfoComponent>
    {
        protected override void Destroy(EquipInfoComponent self)
        {
            self.IsInited = false;
            self.Score    = 0;

            foreach (var entry in self.EntryList)
            {
                AttributeEntry ent = entry;
                ent?.Dispose();
            }
            self.EntryList.Clear();
        }
    }
    
    [FriendOf(typeof(EquipInfoComponent))]
    public class EquipInfoComponentDeserializeSystem : DeserializeSystem<EquipInfoComponent>
    {
        protected override void Deserialize(EquipInfoComponent self)
        {
            foreach (var entity in self.Children.Values)
            {
                self.EntryList.Add(entity as AttributeEntry);
            }
        }
    }

    
    [FriendOf(typeof(ServerItem))]
    [FriendOf(typeof(AttributeEntry))]
    [FriendOf(typeof(EquipInfoComponent))]
    public static class EquipInfoComponentSystem
    {
        public static void GenerateEntries(this EquipInfoComponent self)
        {
            if (self.IsInited)
            {
                return;
            }

            self.IsInited = true;
            self.CreateEntry();
        }
        
        public static void CreateEntry(this EquipInfoComponent self)
        {
            ItemConfig itemConfig = self.GetParent<ServerItem>().Config;
            
            EntryRandomConfig entryRandomConfig = EntryRandomConfigCategory.Instance.Get(itemConfig.EntryRandomId);

            //创建普通词条
            int entryCount = RandomGenerator.RandomNumber(entryRandomConfig.EntryRandMinCount + self.GetParent<ServerItem>().Quality, entryRandomConfig.EntryRandMaxCount + self.GetParent<ServerItem>().Quality);
            for (int i = 0; i < entryCount; i++)
            {
                EntryConfig entryConfig       = EntryConfigCategory.Instance.GetRandomEntryConfigByLevel((int)EntryType.Common,entryRandomConfig.EntryLevel);
                if (entryConfig == null)
                {
                    continue;
                }
                
                AttributeEntry attributeEntry = self.AddChild<AttributeEntry>();
                attributeEntry.Type           = EntryType.Common;
                attributeEntry.Key            = entryConfig.AttributeType;
                attributeEntry.Value          = RandomGenerator.RandomNumber(entryConfig.AttributeMinValue, entryConfig.AttributeMaxValue + self.GetParent<ServerItem>().Quality);
                self.EntryList.Add(attributeEntry);
                self.Score += entryConfig.EntryScore;
            }
            
            
            //创建特殊词条
            entryCount = RandomGenerator.RandomNumber(entryRandomConfig.SpecialEntryRandMinCount, entryRandomConfig.SpecialEntryRandMaxCount );
            for (int i = 0; i < entryCount; i++)
            {
                EntryConfig entryConfig       = EntryConfigCategory.Instance.GetRandomEntryConfigByLevel((int)EntryType.Special,entryRandomConfig.SpecialEntryLevel);
                if (entryConfig == null)
                {
                    continue;
                }
                AttributeEntry attributeEntry = self.AddChild<AttributeEntry>();
                attributeEntry.Type           = EntryType.Special;
                attributeEntry.Key            = entryConfig.AttributeType;
                attributeEntry.Value          = RandomGenerator.RandomNumber(entryConfig.AttributeMinValue, entryConfig.AttributeMaxValue);
                self.EntryList.Add(attributeEntry);
                self.Score += entryConfig.EntryScore;
            }
            
        }
        
        
        public static EquipInfoProto ToMessage(this EquipInfoComponent self)
        {
            EquipInfoProto equipInfoProto =  EquipInfoProto.Create();
            equipInfoProto.Id = self.Id;
            equipInfoProto.Score = self.Score;
            for (int i = 0; i < self.EntryList.Count; i++)
            {
                AttributeEntry ent = self.EntryList[i];
                equipInfoProto.AttributeEntryProtoList.Add(ent.ToMessage());
            }
            return equipInfoProto;
        }
    }
}