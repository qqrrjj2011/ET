﻿using System.Linq;
using ET.EventType;

namespace ET.Client
{
    [FriendOf(typeof(TasksComponent))]
    public class TasksComponentDestroySystem: DestroySystem<TasksComponent>
    {
        protected override void Destroy(TasksComponent self)
        {
            foreach(TaskInfo taskInfo in self.TaskInfoDict.Values)
            {
                taskInfo?.Dispose();
            }
            self.TaskInfoDict.Clear();
        }
    }

    [FriendOf(typeof(TasksComponent))]
    [FriendOf(typeof(TaskInfo))]
    public static class TasksComponentSystem
    {
         public static void AddOrUpdateTaskInfo(this TasksComponent self, TaskInfoProto taskInfoProto)
         {
             TaskInfo taskInfo = self.GetTaskInfoByConfigId(taskInfoProto.ConfigId);
             if ( taskInfo == null )
             {
                 taskInfo = self.AddChild<TaskInfo>();
                 self.TaskInfoDict.Add(taskInfoProto.ConfigId, taskInfo);
             }
             taskInfo.FromMessage(taskInfoProto);
             
             EventSystem.Instance.Publish(self.Root(),new UpdateTaskInfo(){ ZoneScene = self.Root()});
         }


        public static TaskInfo GetTaskInfoByConfigId(this TasksComponent self, int configId)
        {
            self.TaskInfoDict.TryGetValue(configId, out  EntityRef<TaskInfo> taskInfo);
            return taskInfo;
        }

        public static int GetTaskInfoCount(this TasksComponent self)
        {
            self.TaskInfoList.Clear();

            self.TaskInfoList = self.TaskInfoDict.Values.Where(a =>
            {
                TaskInfo info = a;
                return !info.IsTaskState(TaskState.Received);
            }).ToList();
            self.TaskInfoList.Sort((a, b) =>
            {
                TaskInfo info = b;
                return info.TaskState - info.TaskState;
            });
            return self.TaskInfoList.Count;
        }
        
        public static TaskInfo GetTaskInfoByIndex(this TasksComponent self,int index)
        {
            if (index < 0 || index >= self.TaskInfoList.Count)
            {
                return null;
            }
            return self.TaskInfoList[index];
        }
        
        public static bool IsExistTaskComplete(this TasksComponent self)
        {
             foreach (TaskInfo taskInfo in self.TaskInfoDict.Values)
             {
                 if (taskInfo.IsTaskState(TaskState.Complete) )
                 {
                     return true;
                 }
             }
             return false;
        }
    }
}