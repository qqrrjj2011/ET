using System;
using System.Collections.Generic;


namespace ET.Client
{
    [ComponentOf(typeof(Item))]
    // [ChildOf(typeof(AttributeEntry))]

    public class EquipInfoComponent : Entity,IAwake,IDestroy
    {
        public bool IsInited = false;
        
        /// <summary>
        /// 装备评分
        /// </summary>
        public int Score = 0;
        
        /// <summary>
        /// 装备词条列表
        /// </summary>
        public List<EntityRef<AttributeEntry>> EntryList = new List<EntityRef<AttributeEntry>>();
    }
}