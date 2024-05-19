using System.Collections.Generic;
 

namespace ET.Client
{
    [ComponentOf]
     
    public class BagComponent : Entity,IAwake,IDestroy
    {
        public Dictionary<long, EntityRef<Item>> ItemsDict = new Dictionary<long, EntityRef<Item>>(); 
       
        public MultiMap<int, EntityRef<Item>> ItemsMap = new MultiMap<int, EntityRef<Item>>();
        
       // public M2C_ItemUpdateOpInfo message = M2C_ItemUpdateOpInfo() {ContainerType = (int)ItemContainerType.Bag};
       public M2C_ItemUpdateOpInfo message = M2C_ItemUpdateOpInfo.Create();
    }
}