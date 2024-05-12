using System.Linq;

namespace ET.Server
{
    [FriendOf(typeof(PlayerComponent))]
    public static partial class PlayerComponentSystem
    {
        public static void Add(this PlayerComponent self, Player player)
        {
            self.dictionary.Add(player.Account, player);
        }
        
        public static void Remove(this PlayerComponent self, Player player)
        {
            self.dictionary.Remove(player.Account);
            player.Dispose();
        }
        
        /// <summary>
        /// 弃用，使用Get
        /// </summary>
        /// <param name="self"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static Player GetByAccount(this PlayerComponent self, string account)
        {
            // self.dictionary.TryGetValue(accountId, out EntityRef<Player> player);
            // return player;
            return null;
        }
        
        public static Player Get(this PlayerComponent self, string account)
        {
            // if (!self.dictionary.ContainsKey(account))
            // {
            //     return null;
            // }
            self.dictionary.TryGetValue(account, out EntityRef<Player> player);
            return player;
        }
    }
}