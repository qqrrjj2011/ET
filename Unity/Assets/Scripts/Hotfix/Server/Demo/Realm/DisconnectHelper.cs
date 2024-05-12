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
                            // 1111  暂时没有游戏逻辑
                            // var m2GRequestExitGame = (M2G_RequestExitGame)await MessageHelper.CallLocationActor(player.UnitId,new G2M_RequestExitGame());
                            //
                            // //通知聊天服下线聊天Unit
                            // var chat2GRequestExitChat = (Chat2G_RequestExitChat)await MessageHelper.CallActor(player.ChatInfoInstanceId,new G2Chat_RequestExitChat());
                            //
                            // //通知移除账号角色登录信息
                            // long LoginCenterConfigSceneId = StartSceneConfigCategory.Instance.LoginCenterConfig.InstanceId;
                            // var L2G_RemoveLoginRecord     =   (L2G_RemoveLoginRecord) await MessageHelper.CallActor(LoginCenterConfigSceneId, new G2L_RemoveLoginRecord()
                            //                                 {
                            //                                     AccountId = player.AccountId,ServerId = player.DomainZone()
                            //                                 });
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

