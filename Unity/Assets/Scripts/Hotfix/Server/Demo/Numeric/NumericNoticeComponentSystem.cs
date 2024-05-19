using ET.EventType;

namespace ET.Server
{
    [FriendOf(typeof(NumericNoticeComponent))]
    public static class NumericNoticeComponentSystem
    {
        public static void NoticeImmediately(this NumericNoticeComponent self,NumbericChange args)
        {
            Unit unit = self.GetParent<Unit>();
            // ET7多线程网络 使用缓存一次发送多个消息，导致被覆盖，改成下面
            // self.NoticeUnitNumericMessage.UnitId      = unit.Id;
            // self.NoticeUnitNumericMessage.NumericType = args.NumericType;
            // self.NoticeUnitNumericMessage.NewValue    = args.New;
            
            
            M2C_NoticeUnitNumeric NoticeUnitNumericMessage = M2C_NoticeUnitNumeric.Create();
            NoticeUnitNumericMessage.UnitId = unit.Id;
            NoticeUnitNumericMessage.NumericType = args.NumericType;
            NoticeUnitNumericMessage.NewValue = args.New;
            Log.Warning(">>>>>>>>server NumericType:"+args.NumericType + " value:"+args.New);
            MapMessageHelper.SendToClient(unit,NoticeUnitNumericMessage);
        }
    }
}      
        
    
    
    
    
    
    
    
    
    



















        // public long Timer;
        //
        // public long LastNoticeTime;
        //         
        // public long NoticeTime;
        //         
        // public Queue<M2C_NoticeUnitNumeric> messageQueue = new Queue<M2C_NoticeUnitNumeric>();
    
        // public static void Notice(this NumericNoticeComponent self, NumbericChange args)
        // {
        //     if ( !ServerGlobalValue.NoticeImmediatelySet.Contains(args.NumericType) &&  
        //         self.LastNoticeTime > 0 && TimeHelper.ServerNow() - self.LastNoticeTime < 100)
        //     {
        //         self.EnqueueNoticeMessage(args);
        //         self.CheckNoticeTimer();
        //     }
        //     else
        //     {
        //         self.NoticeImmediately(args);
        //     }
        // }
        //
        // public static void EnqueueNoticeMessage(this NumericNoticeComponent self, NumbericChange args)
        // {
        //     M2C_NoticeUnitNumeric message = new M2C_NoticeUnitNumeric()
        //     { 
        //         UnitId = self.GetParent<Unit>().Id, NumericType = args.NumericType, NewValue = args.New
        //     };
        //     self.messageQueue.Enqueue(message);
        // }
        //
        // public static void CheckNoticeTimer(this NumericNoticeComponent self)
        // {
        //     if ( self.NoticeTime >= TimeHelper.ServerNow() )
        //     {
        //       return;
        //     }
        //     
        //     if ( self.Timer != 0 )
        //     {
        //         TimerComponent.Instance.Remove(ref self.Timer);
        //     }
        //     
        //     self.NoticeTime = TimeHelper.ServerNow() + 100;
        //     self.Timer = TimerComponent.Instance.NewOnceTimer(self.NoticeTime, TimerType.NoticeUnitNumericTime, self);
        // }
        //
        //
        //
        // public static void NoticePackMessage(this NumericNoticeComponent self)
        // {
        //     M2C_NoticeUnitNumericPack m2CNoticeUnitNumericPack = new M2C_NoticeUnitNumericPack();
        //     
        //     Unit unit = self.GetParent<Unit>();
        //     m2CNoticeUnitNumericPack.UnitId = unit.Id;
        //     
        //     int queueCount = self.messageQueue.Count;
        //     for ( int i = 0; i < queueCount; ++i )
        //     {
        //         M2C_NoticeUnitNumeric queueMsg = self.messageQueue.Dequeue();
        //         m2CNoticeUnitNumericPack.NoticMessageList.Add(queueMsg);
        //     }
        //
        //     if ( m2CNoticeUnitNumericPack.NoticMessageList.Count > 0 )
        //     {
        //         self.LastNoticeTime = TimeHelper.ServerNow();
        //         MessageHelper.SendToClient(unit,m2CNoticeUnitNumericPack);
        //     }
        // }
        
    // [Timer(TimerType.NoticeUnitNumericTime)]
    // public class UnitNumericNoticeTimerHandler: ATimer<NumericNoticeComponent>
    // {
    //     public override void Run(NumericNoticeComponent numericNoticeComponent)
    //     {
    //         numericNoticeComponent.NoticePackMessage();
    //     }
    // }
