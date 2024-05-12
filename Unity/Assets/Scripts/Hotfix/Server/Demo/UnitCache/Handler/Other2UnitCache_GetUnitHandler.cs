using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ET.Server
{
    [FriendOf(typeof(UnitCacheComponent))]
    [MessageHandler(SceneType.UnitCache)]
    public class Other2UnitCache_GetUnitHandler : MessageHandler<Scene,Other2UnitCache_GetUnit,UnitCache2Other_GetUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_GetUnit request, UnitCache2Other_GetUnit response)
        {
            UnitCacheComponent unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            Dictionary<string,Entity> dictionary =  ObjectPool.Instance.Fetch(typeof (Dictionary<string, Entity>)) as Dictionary<string, Entity>;
            try
            {
                if (request.ComponentNameList == null || request.ComponentNameList.Count == 0)
                {
                    dictionary.Add(nameof (Unit), null);
                    foreach (string s in unitCacheComponent.UnitCacheKeyList)
                    {
                        dictionary.Add(s, null);
                    }
                }
                else
                {
                    foreach (string s in request.ComponentNameList)
                    {
                        dictionary.Add(s, null);
                    }
                }

                // foreach (var key in dictionary.Keys)
                // {
                //     Entity entity = await unitCacheComponent.Get(request.UnitId,key);
                //     dictionary[key] = entity;
                // }
                
                // 上面代码改成这个，防止报错
                // Parallel.ForEach(dictionary.Keys, async key =>
                // {
                //     Entity entity = await unitCacheComponent.Get(request.UnitId, key);
                //     lock (dictionary)
                //     {
                //         dictionary[key] = entity;
                //     }
                // });

                Dictionary<string, Entity> tmpDic = new Dictionary<string, Entity>();
                foreach (var VARIABLE in dictionary)
                {
                    tmpDic.Add(VARIABLE.Key,VARIABLE.Value);
                }

                foreach (var key in tmpDic.Keys)
                {
                    Entity entity = await unitCacheComponent.Get(request.UnitId, key);
                    lock (dictionary)
                    {
                        dictionary[key] = entity;
                    }
                }


                response.ComponentNameList = new List<string>();
                response.ComponentNameList.AddRange(dictionary.Keys);
                response.EntityList = new List<Entity>(); 
                response.EntityList.AddRange(dictionary.Values);
            }
            finally
            {
                dictionary.Clear();
                ObjectPool.Instance.Recycle(dictionary);
            }
            
            // reply();
            await ETTask.CompletedTask;
        }
    }
}