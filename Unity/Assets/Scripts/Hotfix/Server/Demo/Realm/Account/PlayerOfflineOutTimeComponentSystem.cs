using System;
using System.Collections.Generic;
using System.Text;

namespace ET.Server
{
    [FriendOf(typeof(PlayerOfflineOutTimeComponent))]
    public static class PlayerOfflineOutTimeComponentSystem
    {
        [Invoke(TimerInvokeType.PlayerOfflineOutTime)]
        public class PlayerOfflineOutTime: ATimer<PlayerOfflineOutTimeComponent>
        {
            protected override void Run(PlayerOfflineOutTimeComponent self)
            {
                try
                {
                    self.KickPlayer();
                }
                catch (Exception e)
                {
                    Log.Error($"playerOffline timer error: {self.Id}\n{e}");
                }
            }
        }
    }
    
    [FriendOf(typeof(PlayerOfflineOutTimeComponent))]
    [EntitySystem]
    public class GateUnitDeleteComponentDestroySystem : DestroySystem<PlayerOfflineOutTimeComponent>
    {
        protected override void Destroy(PlayerOfflineOutTimeComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }
    }


    [FriendOf(typeof(PlayerOfflineOutTimeComponent))]
    [EntitySystem]
    public class PlayerOfflineOutTimeComponentAwakeSystem : AwakeSystem<PlayerOfflineOutTimeComponent>
    {
        protected override void Awake(PlayerOfflineOutTimeComponent self)
        {
            self.Timer = self.Root().GetComponent<TimerComponent>().NewOnceTimer(TimeInfo.Instance.ServerNow() + 10000,TimerInvokeType.PlayerOfflineOutTime,self);
        }
    }
    
    public static class GateUnitDeleteComponentSystem
    {
        public static void KickPlayer(this PlayerOfflineOutTimeComponent self)
        {
            DisconnectHelper.KickPlayer(self.GetParent<Player>()).Coroutine();
        }
    }
    
}

