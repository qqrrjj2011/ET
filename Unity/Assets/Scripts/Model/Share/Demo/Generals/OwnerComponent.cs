namespace ET
{
    /// <summary>
    /// 所有者。将领归属那个unit
    /// </summary>
    [ComponentOf]
    public class OwnerComponent: Entity,IAwake
    {
        /// <summary>
        /// 所有者id
        /// </summary>
        public long ownerId;
    }
}

