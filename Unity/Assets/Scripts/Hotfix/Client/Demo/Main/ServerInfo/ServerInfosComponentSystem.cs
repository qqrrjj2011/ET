namespace ET.Client
{
    [EntitySystemOf(typeof(ClientServerInfosComponent))]
    [FriendOf(typeof(ClientServerInfosComponent))]
    public static partial class ClientServerInfosComponentSystem
    {

        public static void Add(this ClientServerInfosComponent self, ServerInfo serverInfo)
        {
            self.ServerInfoList.Add(serverInfo);
        }
        [EntitySystem]
        private static void Awake(this ET.Client.ClientServerInfosComponent self)
        {

        }
        [EntitySystem]
        private static void Destroy(this ET.Client.ClientServerInfosComponent self)
        {
            foreach (ServerInfo serverInfo in self.ServerInfoList)
            {
                if(serverInfo != null && !serverInfo.IsDisposed)   
                    serverInfo.Dispose();
            }

            self.ServerInfoList.Clear();

            self.CurrentServerId = 0;
        }
    }
}