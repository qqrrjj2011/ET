using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Client
{
    [ComponentOf]
   // [ChildOf(typeof(TaskInfo))]
    public class TasksComponent : Entity,IAwake,IDestroy
    {

        public SortedDictionary<int,EntityRef<TaskInfo>> TaskInfoDict = new SortedDictionary<int, EntityRef<TaskInfo>>();

        public List<EntityRef<TaskInfo>> TaskInfoList = new List<EntityRef<TaskInfo>>();
        

        
    }
}