namespace ET.Client
{
    [FriendOf(typeof(ClientServerInfosComponent))]
    public class ClientServerInfosComponentDestroySystem : DestroySystem<ClientServerInfosComponent>
    {
        protected override void Destroy(ClientServerInfosComponent self)
        {
            foreach (ServerInfo serverInfo in self.ServerInfoList)
            {
                serverInfo.Dispose();
            }

            self.ServerInfoList.Clear();

            self.CurrentServerId = 0;
        }
    }
    [FriendOf(typeof(ClientServerInfosComponent))]
    public static class ClientServerInfosComponentSystem
    {
     
        public static void Add(this ClientServerInfosComponent self, ServerInfo serverInfo)
        {
            self.ServerInfoList.Add(serverInfo);
        }
    }
}