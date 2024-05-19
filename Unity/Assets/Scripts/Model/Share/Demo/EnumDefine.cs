namespace ET
{
    /// <summary>
    /// 物品项类型
    /// </summary>
    public enum ItemType
    {
        Weapon   = 0, //武器
        Armor    = 1, //防具
        Ring     = 2, //戒指
        Prop     = 3, //道具
    }

    /// <summary>
    /// 物品操作指令
    /// </summary>
    public enum ItemOp
    {
        Add = 0,  //增加物品
        Remove = 1 //移除物品
    }


    /// <summary>
    /// 物品容器类型
    /// </summary>
    public enum ItemContainerType
    {
        Bag = 0,  //背包容器
        RoleInfo = 1, //游戏角色装配容器
    }
    
    
    
    
    /// <summary>
    /// 物品品质等级
    /// </summary>
    public enum ItemQuality
    {
        General   = 0, //普通
        Good      = 1, //良好
        Excellent = 2, //精良
        Epic      = 3, //史诗
        Legend    = 4, //传奇
    }
    
}