using ET.Client;

namespace ET.Event
{
    [Event(SceneType.Current)]
    public class SceneChangeFinish_StartAdventure: AEvent<Scene, EventType.SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, EventType.SceneChangeFinish args)
        {
            Unit unit = UnitHelper.GetMyUnitFromCurrentScene(args.CurrentScene);

            if (unit.GetComponent<NumericComponent>().GetAsInt(NumericType.AdventureState) == 0)
            {
                return;
            }

            await scene.GetComponent<TimerComponent>().WaitAsync(3000);
            
           // args.CurrentScene.GetComponent<AdventureComponent>().StartAdventure().Coroutine();
            scene.GetComponent<AdventureComponent>().StartAdventure().Coroutine();
            await ETTask.CompletedTask;
        }
    }
}