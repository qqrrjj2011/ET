namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_CreateGeneralsHandler:MessageHandler<Scene,M2C_CreateGeneralsUnits>
    {
        protected override async ETTask Run(Scene scene, M2C_CreateGeneralsUnits message)
        {
            Scene curScene = scene.GetComponent<CurrentScenesComponent>().Scene;
            Log.Warning(">>>>>>>>>M2C_CreateGeneralsHandler run");
            for (int i = 0; i < message.Units.Count; i++)
            {
                Unit unit = UnitFactory.Create(curScene, message.Units[i]);
                curScene.GetComponent<UnitComponent>().Add(unit);
            }
            await ETTask.CompletedTask;
        }
    }
    
}

