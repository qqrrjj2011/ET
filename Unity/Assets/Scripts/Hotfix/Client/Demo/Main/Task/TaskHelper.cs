using System;

namespace ET.Client
{
    public static class TaskHelper
    {
        public static async ETTask<int>  GetTaskReward(Scene ZoneScene, int taskConfigId)
        {
            TaskInfo taskInfo = ZoneScene.GetComponent<TasksComponent>().GetTaskInfoByConfigId(taskConfigId);

            if ( taskInfo == null || taskInfo.IsDisposed )
            {
                return ErrorCode.ERR_NoTaskInfoExist;
            }

            if ( !taskInfo.IsTaskState(TaskState.Complete) )
            {
                return ErrorCode.ERR_TaskNoCompleted;
            }

             M2C_ReceiveTaskReward m2CReciveTaskReward = null;
             try
             {
                 C2M_ReceiveTaskReward c2MReceiveTaskReward = C2M_ReceiveTaskReward.Create();
                 c2MReceiveTaskReward.TaskConfigId = taskConfigId;
                 m2CReciveTaskReward = (M2C_ReceiveTaskReward)await ZoneScene.GetComponent<SessionComponent>().Session.Call(c2MReceiveTaskReward);
             }
             catch (Exception e)
             {
                 Log.Error(e.ToString());
                 return ErrorCode.ERR_NetWorkError;
             }
            
            return m2CReciveTaskReward.Error;
        
        }
    }
}