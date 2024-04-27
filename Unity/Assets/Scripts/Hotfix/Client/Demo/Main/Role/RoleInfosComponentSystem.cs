using System.Collections.Generic;

namespace ET.Client
{
 
    [FriendOf(typeof(RoleInfosComponent))]
    public class RoleInfosComponentDestroySystem: DestroySystem<RoleInfosComponent>
    {
        protected override void Destroy(RoleInfosComponent self)
        {
            foreach (RoleInfo roleInfo in self.RoleInfos)
            {
                roleInfo?.Dispose();
            }
            self.RoleInfos.Clear();
            self.CurrentRoleId = 0;
        }
    }

    public static class RoleInfosComponentSystem
    {
        
        
    }
}