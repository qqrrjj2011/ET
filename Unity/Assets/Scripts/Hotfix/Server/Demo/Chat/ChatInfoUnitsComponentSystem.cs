using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(ChatInfoUnitsComponent))]
    public class ChatInfoUnitsComponentDestroy : DestroySystem<ChatInfoUnitsComponent>
    {
        protected override void Destroy(ChatInfoUnitsComponent self)
        {
            foreach (var chatInfoUnit in self.ChatInfoUnitsDict.Values)
            {
                ChatInfoUnit ent = chatInfoUnit;
                ent?.Dispose();
            }
        }
    }

    [FriendOf(typeof(ChatInfoUnitsComponent))]
    public static class ChatInfoUnitsComponentSystem
    {
        public static void Add(this ChatInfoUnitsComponent self, ChatInfoUnit chatInfoUnit)
        {
            if (!self.ChatInfoUnitsDict.TryAdd(chatInfoUnit.Id, chatInfoUnit))
            {
                Log.Error($"chatInfoUnit is exist! ： {chatInfoUnit.Id}");
                return;
            }
        }


        public static ChatInfoUnit Get(this ChatInfoUnitsComponent self, long id)
        {
            self.ChatInfoUnitsDict.TryGetValue(id, out EntityRef<ChatInfoUnit> chatInfoUnit);
            return chatInfoUnit;
        }


        public static void Remove(this ChatInfoUnitsComponent self, long id)
        {
            if (self.ChatInfoUnitsDict.Remove(id, out EntityRef<ChatInfoUnit> chatInfoUnit))
            {
                ChatInfoUnit ent = chatInfoUnit;
                ent?.Dispose();
            }
        }
    }
}