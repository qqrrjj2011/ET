
namespace ET.Client
{
    [ChildOf]
    public class Item : Entity,IAwake<int>,IDestroy
    {
        //物品配置ID
        public int ConfigId = 0;

        /// <summary>
        /// 物品品质
        /// </summary>
        public int Quality  = 0;
        
        //物品配置数据
        public ItemConfig Config => ItemConfigCategory.Instance.Get(ConfigId);
        
    }
}