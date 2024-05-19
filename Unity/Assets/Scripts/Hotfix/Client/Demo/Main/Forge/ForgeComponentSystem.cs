using ET.EventType;

namespace ET.Client
{
    [Invoke(TimerInvokeType.MakeQueueOver)]
    public class MakeQueueOverTimer : ATimer<ForgeComponent>
    {
        protected override void Run(ForgeComponent t)
        {
            EventSystem.Instance.Publish(t.Root(), MakeQueueOver.Instance);
        }
 
    }
   
    
     public class ForgeComponentAwakeSystem : AwakeSystem<ForgeComponent>
     {
         protected override void Awake(ForgeComponent self)
         {
          
         }
     }
     
     [FriendOf(typeof(ForgeComponent))]
     public class ForgeComponentDestroySystem : DestroySystem<ForgeComponent>
     {
         protected override void Destroy(ForgeComponent self)
         {
             
             foreach (Production production in self.ProductionsList)
             {
                 production?.Dispose();
             }

             ForeachHelper.Foreach<long,long>(self.ProductionTimerDict, (key, value) =>
             {
                 self.Root().GetComponent<TimerComponent>()?.Remove(ref value);
             });
         }
     }
     
    [FriendOf(typeof(ForgeComponent))]
    [FriendOf(typeof(Production))]
    public static class ForgeComponentSystem
    {
        public static bool IsExistMakeQueueOver(this ForgeComponent self)
        {
            bool isCanRecive = false;
    
            for (int i = 0; i < self.ProductionsList.Count; i++)
            {
                Production production = self.ProductionsList[i];
                if (production.IsMakingState() && production.IsMakeTimeOver())
                {
                    isCanRecive = true;
                    break;
                }
            }
            return isCanRecive;
        }
        
        
        public static void AddOrUpdateProductionQueue(this ForgeComponent self, ProductionProto productionProto)
        {
            Production production = self.GetProductionById(productionProto.Id);

            if (production == null)
            {
                production = self.AddChild<Production>();
                self.ProductionsList.Add(production);
            }
            production.FromMessage(productionProto);

            
            if (self.ProductionTimerDict.TryGetValue(production.Id,out long timeId))
            {
                self.Root().GetComponent<TimerComponent>().Remove(ref timeId);
                self.ProductionTimerDict.Remove(production.Id);
            }

           
            if ( production.IsMakingState() && !production.IsMakeTimeOver() )
            {
                Log.Debug($"启动一个定时器!!!!:{production.TargetTime}");
                timeId = self.Root().GetComponent<TimerComponent>().NewOnceTimer(production.TargetTime, TimerInvokeType.MakeQueueOver, self);
                self.ProductionTimerDict.Add(production.Id,timeId);
            }

            EventSystem.Instance.Publish(self.Root(), MakeQueueOver.Instance);
        }


        public static Production GetProductionById(this ForgeComponent self, long productionId)
        {
            for (int i = 0; i < self.ProductionsList.Count; i++)
            {
                Production ent = self.ProductionsList[i];
                if ( ent.Id == productionId )
                {
                    return ent;
                }
            }
            return null;
        }
        
        public static Production GetProductionByIndex(this ForgeComponent self,int index)
        {
            for (int i = 0; i < self.ProductionsList.Count; i++)
            {
                if ( index == i )
                {
                    return self.ProductionsList[i];
                }
            }
            return null;
        }

        public static int GetMakeingProductionQueueCount(this ForgeComponent self)
        {
            int count = 0;
            for (int i = 0; i < self.ProductionsList.Count; i++)
            {
                Production production = self.ProductionsList[i];
                if (production.ProductionState == (int)ProductionState.Making)
                {
                    ++count;
                }
            }
            return count;
        }


    }
}