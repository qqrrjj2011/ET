using ET.EventType;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class BattleWinEvent_TaskUpdate : AEvent<Scene, BattleWin>
    {
        protected override async ETTask Run(Scene scene, BattleWin args)
        {
            args.Unit.GetComponent<ServerTasksComponent>().TriggerTaskAction(TaskActionType.Adverture,count:1, targetId : args.LevelId);
            await ETTask.CompletedTask;
        }
 
    }
}