
namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOfAttribute(typeof(ET.GeneralsComponent))]
    public class C2M_PathfindingResultHandler : MessageLocationHandler<Unit, C2M_PathfindingResult>
    {
        protected override async ETTask Run(Unit unit, C2M_PathfindingResult message)
        {
            unit.FindPathMoveToAsync(message.Position,false).Coroutine();
            
            // 将领跟随移动
            for (int i = 0; i < unit.GetComponent<GeneralsComponent>().generalsIds.Count; i++)
            {
                Unit generalsUnit = unit.Root().GetComponent<UnitComponent>().GetChild<Unit>(unit.GetComponent<GeneralsComponent>().generalsIds[i]);
                generalsUnit.MoveFollw(message.Position,false).Coroutine();
            }

            await ETTask.CompletedTask;
        }
    }
}