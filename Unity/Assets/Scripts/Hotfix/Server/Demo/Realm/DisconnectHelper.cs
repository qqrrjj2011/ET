namespace ET.Server
{
    public static class DisconnectHelper
    {
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }

            long curInstanceId = self.InstanceId;
            TimerComponent timerComponent = self.Root().GetComponent<TimerComponent>();
            await timerComponent.WaitAsync(1000);

            if (self.IsDisposed || curInstanceId != self.InstanceId)
            {
                return;
            }
            self.Dispose();
        }
        
        public static async ETTask KickPlayer(Player player,bool isException = false)
        {
            if (player == null || player.IsDisposed)
            {
                return;
            }
            long instanceId = player.InstanceId;
            using (await player.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.LoginGate, player.Account.GetHashCode()))
            {
                if (player.IsDisposed || instanceId != player.InstanceId)
                {
                    return;
                }

                if (!isException)
                {
                    switch (player.PlayerState)
                    {
                        case PlayerState.Disconnect:
                            break;
                        case PlayerState.Gate:
                            break;
                        case PlayerState.Game:
                            //通知游戏逻辑服下线Unit角色逻辑，并将数据存入数据库
                            var m2GRequestExitGame = (M2G_RequestExitGame)await player.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Call(player.UnitId,G2M_RequestExitGame.Create());
                            //通知聊天服下线聊天Unit
                            var chat2GRequestExitChat = (Chat2G_RequestExitChat) await player.Root().GetComponent<MessageSender>().Call(player.ChatInfoActorId, G2Chat_RequestExitChat.Create());
                            
                            //通知移除账号角色登录信息
                            ActorId LoginCenterConfigSceneActorId = StartSceneConfigCategory.Instance.LoginCenterConfig.ActorId;
                            G2L_RemoveLoginRecord g2LRemoveLoginRecord = G2L_RemoveLoginRecord.Create();
                            g2LRemoveLoginRecord.Account = player.Account;
                            g2LRemoveLoginRecord.ServerId = player.Zone();
                            var L2G_RemoveLoginRecord     =   (L2G_RemoveLoginRecord) await player.Root().GetComponent<MessageSender>().Call(LoginCenterConfigSceneActorId,  g2LRemoveLoginRecord);
                            break;
                    }
                }
                TimerComponent timerComponent = player.Root().GetComponent<TimerComponent>();
                player.Root().GetComponent<PlayerComponent>()?.Remove(player);
                player?.Dispose();
                await timerComponent.WaitAsync(300);
            }
        }
    }
}

