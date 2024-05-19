namespace ET.Server
{
    public static class BagHelper
    {
        public static bool AddItemByConfigId(Unit unit, int configId)
        {
            ServerBagComponent bagComponent = unit.GetComponent<ServerBagComponent>();
            if ( bagComponent == null)
            {
                return false;
            }

            if (!bagComponent.IsCanAddItemByConfigId(configId))
            {
                return false;
            }

            return bagComponent.AddItemByConfigId(configId);
        }
    }
}