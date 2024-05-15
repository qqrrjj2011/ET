using System;
using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(RankInfosComponent))]
    [MessageHandler(SceneType.Rank)]
    public class C2Rank_GetRanksInfoHandler : MessageHandler<Scene, C2Rank_GetRanksInfo, Rank2C_GetRanksInfo>
    {
        protected override async ETTask Run(Scene scene, C2Rank_GetRanksInfo request, Rank2C_GetRanksInfo response)
        {
            RankInfosComponent rankInfosComponent = scene.GetComponent<RankInfosComponent>();
            int count = 0;
            if (rankInfosComponent.SortedRankInfoList != null)
            {
                foreach (var rankInfo in rankInfosComponent.SortedRankInfoList)
                {
                    if (response.RankInfoProtoList == null) response.RankInfoProtoList = new List<RankInfoProto>();
                    RankInfo ent = rankInfo.Key;
                    response.RankInfoProtoList.Add(ent.ToMessage());
                    ++count;
                    if (count >= 100)
                    {
                        break;
                    }
                }
            }

            
            
           // reply();
            await ETTask.CompletedTask;
        }
    }
}