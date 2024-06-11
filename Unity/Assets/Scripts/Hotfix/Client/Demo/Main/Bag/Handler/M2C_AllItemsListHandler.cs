
namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_AllItemsListHandler: MessageHandler<Scene,M2C_AllItemsList>
    {
        protected override async ETTask Run(Scene Scene, M2C_AllItemsList message)
        {
            ItemHelper.Clear(Scene.Root(),(ItemContainerType)message.ContainerType);
            
            if(message.ItemInfoList != null)
                for (int i = 0; i < message.ItemInfoList.Count; i++)
                {
                    Item item = ItemFactory.Create(Scene.Root(),message.ItemInfoList[i]);
                    ItemHelper.AddItem(Scene.Root(),item,(ItemContainerType)message.ContainerType);
                }

            await ETTask.CompletedTask;
        }
    }
}