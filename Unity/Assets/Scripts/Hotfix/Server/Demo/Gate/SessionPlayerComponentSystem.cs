namespace ET.Server
{
    [EntitySystemOf(typeof(SessionPlayerComponent))]
    [FriendOf(typeof(SessionPlayerComponent))]
    public static partial class SessionPlayerComponentSystem
    {
        [EntitySystem]
        private static void Destroy(this SessionPlayerComponent self)
        {
            // Scene root = self.Root();
            // if (root.IsDisposed)
            // {
            //     return;
            // }
            // // 发送断线消息
            // root.GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Send(self.Player.UnitId, G2M_SessionDisconnect.Create());
            if (self.IsDisposed)
            {
                return;
            }

            // 发送断线消息
            if (!self.isLoginAgain && !self.Player.IsDisposed)
            {
                Player player = self.Player;
                DisconnectHelper.KickPlayer(player).Coroutine();
            }

            self.isLoginAgain = false;
        }
        
        public static Player GetMyPlayer(this SessionPlayerComponent self)
        {
            return self.Player;
        }
        
        [EntitySystem]
        private static void Awake(this SessionPlayerComponent self)
        {

        }
    }
}