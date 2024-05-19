using System;

namespace ET.Server
{
     [MessageLocationHandler(SceneType.Map)]
     public class C2M_ReceiveTaskRewardHandler : MessageLocationHandler<Unit,C2M_ReceiveTaskReward,M2C_ReceiveTaskReward>
     {
         protected override async ETTask Run(Unit unit, C2M_ReceiveTaskReward request, M2C_ReceiveTaskReward response)
         {
             ServerTasksComponent tasksComponent = unit.GetComponent<ServerTasksComponent>();
             
             int errorCode = tasksComponent.TryReceiveTaskReward(request.TaskConfigId);
             if (errorCode != ErrorCode.ERR_Success)
             {
                 response.Error = errorCode;
               //  reply();
                 return;
             }
         
             tasksComponent.ReceiveTaskRewardState(unit,request.TaskConfigId);
    
             unit.GetComponent<NumericComponent>()[NumericType.Gold] += TaskConfigCategory.Instance.Get(request.TaskConfigId).RewardGoldCount;
             
           //  reply();
             
             await ETTask.CompletedTask;
         }
     }
}