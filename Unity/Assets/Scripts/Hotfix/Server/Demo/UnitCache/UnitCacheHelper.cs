using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    public static class UnitCacheHelper
    {
        /// <summary>
        /// 保存或者更新玩家缓存
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        public static async ETTask AddOrUpdateUnitCache<T>(this T self) where T : Entity, IUnitCache
        {
            Other2UnitCache_AddOrUpdateUnit message = Other2UnitCache_AddOrUpdateUnit.Create();
            message.UnitId = self.Id;
            message.EntityTypes = new List<string>();
            message.EntityBytes = new List<byte[]>();
            message.EntityTypes.Add(typeof (T).FullName);
            message.EntityBytes.Add(MongoHelper.Serialize(self));
            await self.Root().GetComponent<MessageSender>().Call(StartSceneConfigCategory.Instance.GetUnitCacheConfig(self.Id).ActorId, message);
        }
        
        /// <summary>
        /// 获取玩家缓存
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static async ETTask<Unit> GetUnitCache(Scene scene, long unitId)
        {
            Other2UnitCache_GetUnit message = Other2UnitCache_GetUnit.Create();
            message.UnitId = unitId;
            UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit) await scene.Root().GetComponent<MessageSender>().Call(StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).ActorId,message);
            if (queryUnit.Error != ErrorCode.ERR_Success ||  queryUnit.EntityList == null || queryUnit.EntityList.Count <= 0)
            {
                return null;
            }

            int indexOf = queryUnit.ComponentNameList.IndexOf(typeof(Unit).FullName);
            Unit unit = queryUnit.EntityList[indexOf] as Unit;
            if (unit == null)
            {
                return null;
            }
            scene.GetComponent<UnitComponent>().AddChild(unit);
            foreach (Entity entity in queryUnit.EntityList)
            {
                if (entity == null || entity is Unit)
                {
                    continue;
                }
                unit.AddComponent(entity);
            }
            return unit;
        }

        /// <summary>
        /// 获取玩家组件缓存
        /// </summary>
        /// <param name="unitId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async ETTask<T> GetUnitComponentCache<T>(long unitId) where T : Entity, IUnitCache
        {
            // Other2UnitCache_GetUnit message = new Other2UnitCache_GetUnit() { UnitId = unitId };
            // message.ComponentNameList = new List<string>();
            // message.ComponentNameList.Add(typeof (T).Name);
            // ActorId actorId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).ActorId;
            // UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit) await MessageHelper.CallActor(actorId, message);
            // if (queryUnit.Error == ErrorCode.ERR_Success && queryUnit.EntityList !=null && queryUnit.EntityList.Count > 0)
            // {
            //     return queryUnit.EntityList[0] as T;
            // }
            await ETTask.CompletedTask;
            return null;
        }
        
        /// <summary>
        /// 删除玩家缓存
        /// </summary>
        /// <param name="unitId"></param>
        public static async ETTask DeleteUnitCache(long unitId)
        {
            // Other2UnitCache_DeleteUnit message = new Other2UnitCache_DeleteUnit() { UnitId = unitId };
            // await MessageHelper.CallActor(StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).ActorId, message);
            await ETTask.CompletedTask;
        }
        
        
        /// <summary>
        /// 保存Unit及Unit身上组件到缓存服及数据库中
        /// </summary>
        /// <param name="unit"></param>
        public static void AddOrUpdateUnitAllCache(Unit unit)
        {
            Other2UnitCache_AddOrUpdateUnit message = Other2UnitCache_AddOrUpdateUnit.Create();
            message.UnitId = unit.Id;
            message.EntityTypes = new List<string>();
            message.EntityBytes = new List<byte[]>();
            message.EntityTypes.Add(unit.GetType().FullName);
            message.EntityBytes.Add(MongoHelper.Serialize(unit));
            
            foreach ((long key,Entity entity) in unit.Components)
            {
                //1111 if (!typeof (IUnitCache).IsAssignableFrom(entity.GetType()))
                if (!(entity is IUnitCache))
                {
                    continue;
                }
                //111 message.EntityTypes.Add(key.FullName);
                message.EntityTypes.Add(entity.GetType().FullName);
                message.EntityBytes.Add(MongoHelper.Serialize(entity));
            }
            unit.Root().GetComponent<MessageSender>().Call(StartSceneConfigCategory.Instance.GetUnitCacheConfig(unit.Id).ActorId, message).Coroutine();
        }
    }
}