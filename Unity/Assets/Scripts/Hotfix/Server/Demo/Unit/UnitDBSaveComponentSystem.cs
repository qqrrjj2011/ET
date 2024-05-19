using System;
using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(UnitDBSaveComponent))]
    public class UnitDBSaveComponentAwakeSystem: AwakeSystem<UnitDBSaveComponent>
    {
        protected override void Awake(UnitDBSaveComponent self)
        {
            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(10000, TimerInvokeType.SaveChangeDBData, self);
        }
    }
    
    [FriendOf(typeof(UnitDBSaveComponent))]
    public class UnitDBSaveComponentDestroySystem: DestroySystem<UnitDBSaveComponent>
    {
        protected override void Destroy(UnitDBSaveComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }
    }
    
    public  class UnitAddComponentSystem : AddComponentSystem<Unit>
    {
        protected override void AddComponent(Unit unit, Entity component)
        { 
            Type type = component.GetType();
            if (!(typeof (IUnitCache).IsAssignableFrom(type)) )
            {
                return;
            }
            unit.GetComponent<UnitDBSaveComponent>()?.AddChange(type);
        }
    }
    
    public class UnitGetComponentSystem : GetComponentSysSystem<Unit>
    {
        protected override void GetComponentSys(Unit unit, Type type)
        {
            if (!(typeof (IUnitCache).IsAssignableFrom(type)) )
            {
                return;
            }
            unit.GetComponent<UnitDBSaveComponent>()?.AddChange(type);
        }
    }

    
    [Invoke(TimerInvokeType.SaveChangeDBData)]
    public class UnitDBSaveComponentTimer : ATimer<UnitDBSaveComponent>
    {
        protected override void Run(UnitDBSaveComponent self)
        {
            try
            {
                if ( self.IsDisposed || self.Parent == null )
                {
                    return;
                }
                
                if ( self.Root() == null )
                {
                    return;
                }

                self?.SaveChange();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }
    
    
    public static class UnitDBSaveComponentSystem
    {
        public static void AddChange(this UnitDBSaveComponent self,Type t)
        {
            self.EntityChangeTypeSet.Add(t);
        }
        
        public static void SaveChange(this UnitDBSaveComponent self)
        {
            if (self.EntityChangeTypeSet.Count <= 0)
            {
                return;
            }
            Unit unit = self.GetParent<Unit>();
            Other2UnitCache_AddOrUpdateUnit message = Other2UnitCache_AddOrUpdateUnit.Create();
            message.UnitId = unit.Id;
            message.EntityTypes = new List<string>();
            message.EntityBytes = new List<byte[]>();
            message.EntityTypes.Add(unit.GetType().FullName);
            message.EntityBytes.Add(MongoHelper.Serialize(unit));
            foreach (Type type in self.EntityChangeTypeSet)
            {
                Entity entity = unit.GetComponent(type);
                if ( entity == null || entity.IsDisposed )
                {
                    continue;
                }
                Log.Debug("开始保存变化部分的Entity数据 : " + type.FullName);
                message.EntityTypes.Add(type.FullName);
                message.EntityBytes.Add(MongoHelper.Serialize(entity));
            }
            self.EntityChangeTypeSet.Clear();
            
            // MessageHelper.CallActor(StartSceneConfigCategory.Instance.GetUnitCacheConfig(unit.Id).InstanceId, message).Coroutine();
            
            self.Root().GetComponent<MessageSender>().Call(StartSceneConfigCategory.Instance.GetUnitCacheConfig(unit.Id).ActorId,message).Coroutine();
        }
    }
}