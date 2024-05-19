namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_ItemUpdateOpInfoHandler: MessageHandler<Scene,M2C_ItemUpdateOpInfo>
    {
        protected override async ETTask  Run(Scene scene, M2C_ItemUpdateOpInfo message)
        {
            if (message.Op ==(int)ItemOp.Add)
            {
                Item item = ItemFactory.Create(scene.Root(),message.ItemInfo);
                ItemHelper.AddItem(scene.Root(),item,(ItemContainerType)message.ContainerType);
            }
            else if (message.Op == (int) ItemOp.Remove)
            {
                ItemHelper.RemoveItemById(scene.Root(),message.ItemInfo.ItemUid,(ItemContainerType)message.ContainerType);
            }

            await ETTask.CompletedTask;
        }
    }
}