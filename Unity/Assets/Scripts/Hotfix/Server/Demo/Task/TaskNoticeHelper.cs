namespace ET.Server
{
    [FriendOf(typeof(ServerTasksComponent))]
    public static class TaskNoticeHelper
    {
         public static void SyncTaskInfo(Unit unit, TaskInfo taskInfo, M2C_UpdateTaskInfo updateTaskInfo)
         {
             updateTaskInfo.TaskInfoProto = taskInfo.ToMessage();
             MapMessageHelper.SendToClient(unit, updateTaskInfo);
         }
        
         public static void SyncAllTaskInfo(Unit unit)
         {
             ServerTasksComponent tasksComponent = unit.GetComponent<ServerTasksComponent>();
             
             M2C_AllTaskInfoList m2CAllTaskInfoList = M2C_AllTaskInfoList.Create();
        
             foreach (TaskInfo taskInfo in tasksComponent.TaskInfoDict.Values)
             {
                 
                 m2CAllTaskInfoList.TaskInfoProtoList.Add(taskInfo.ToMessage());
             }
             MapMessageHelper.SendToClient(unit, m2CAllTaskInfoList);
         }
        
    }
}