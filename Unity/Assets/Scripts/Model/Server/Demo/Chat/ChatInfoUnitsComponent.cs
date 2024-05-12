using System;
using System.Collections.Generic;

namespace ET.Server
{
  //  [ChildOf(typeof(ChatInfoUnit))]
    [ComponentOf(typeof(Scene))]
    public class ChatInfoUnitsComponent : Entity,IAwake,IDestroy
    {
        public  Dictionary<long, EntityRef<ChatInfoUnit>> ChatInfoUnitsDict = new Dictionary<long, EntityRef<ChatInfoUnit>>();
    }
}