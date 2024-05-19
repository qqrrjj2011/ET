using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ET.Server
{
    [EntitySystemOf(typeof(ServerInfosComponent))]
    [FriendOf(typeof(ServerInfosComponent))]
    [FriendOf(typeof(ServerInfo))]
    public static partial class ServerInfosComponentSystem
    {
        [EntitySystem]
        private  static void Awake(this ServerInfosComponent self)
        {
            // 这里写到ServerInfoManagerComponentSystem中
            self.GetServerInfo().Coroutine();
        }

        static async ETTask GetServerInfo(this ServerInfosComponent self)
        {
          // DBComponent dbComponent = self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone());
          // var infoList = await dbComponent.Query<ServerInfo>(x=>true);
          //   if (infoList != null && infoList.Count > 0)
          //   {
          //       foreach (var VARIABLE in infoList)
          //       {
          //           self.ServerInfoList.Add(VARIABLE);
          //       }
          //   }
          //   else
          //   {
          //       infoList.Clear();
          //       var allServers = ServerInfoConfigCategory.Instance.GetAll();
          //       foreach (var VARIABLE in allServers.Values)
          //       {
          //           ServerInfo info = self.AddChildWithId<ServerInfo>(VARIABLE.Id);
          //           info.ServerName = VARIABLE.ServerName;
          //           info.Status = (int)ServerStatus.Normal;
          //           self.ServerInfoList.Add(info);
          //           await self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Save(info);
          //       }
          //   }

          if (self.ServerInfoList.Count > 0)
          {
              foreach (var VARIABLE in self.ServerInfoList)
              {
                  ServerInfo info = VARIABLE;
                  info.Dispose();
              }
              self.ServerInfoList.Clear();
          }

          var allData = StartZoneConfigCategory.Instance.GetAll();
          foreach (var VARIABLE in allData)
          {
              if (VARIABLE.Value.ZoneType != 1)
              {
                  continue;
              }

              ServerInfo info = self.AddChildWithId<ServerInfo>(VARIABLE.Value.Id);
              info.Status = ServerStatus.Normal;
              info.ServerName = VARIABLE.Value.ServerName;
              self.ServerInfoList.Add(info);
          }

          await ETTask.CompletedTask;
        }

        [EntitySystem]
        private static void Destroy(this ServerInfosComponent self)
        {
            foreach (var VARIABLE in self.ServerInfoList)
            {
                ServerInfo info = VARIABLE;
                info?.Dispose();
            }
            self.ServerInfoList.Clear();
        }


    }
    
}

