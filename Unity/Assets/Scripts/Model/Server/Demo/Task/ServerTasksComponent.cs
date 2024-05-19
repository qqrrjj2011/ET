using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf]
   // [ChildOf(typeof(TaskInfo))]
    public class ServerTasksComponent : Entity,IAwake,IDestroy,ITransfer,IUnitCache, IDeserialize
    {
        [BsonIgnore]
        public SortedDictionary<int,EntityRef<TaskInfo>> TaskInfoDict = new SortedDictionary<int, EntityRef<TaskInfo>>();

        [BsonIgnore]
        public HashSet<int> CurrentTaskSet = new HashSet<int>();
        
         [BsonIgnore]
         public M2C_UpdateTaskInfo M2CUpdateTaskInfo = M2C_UpdateTaskInfo.Create();
        
    }
}