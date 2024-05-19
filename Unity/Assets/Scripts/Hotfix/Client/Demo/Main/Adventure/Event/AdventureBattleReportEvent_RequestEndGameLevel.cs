
using ET.Client;

namespace ET.Event
{
    [Event(SceneType.Demo)]
    public class AdventureBattleReportEvent_RequestEndGameLevel : AEvent<Scene,EventType.AdventureBattleReport>
    {
        protected override async ETTask Run(Scene scene,EventType.AdventureBattleReport args)
        {
            if (args.BattleRoundResult == BattleRoundResult.KeepBattle)
            {
                return;
            }
            
            int errCode = await AdventureHelper.RequestEndGameLevel(args.scene,args.BattleRoundResult,args.Round);

            if (errCode != ErrorCode.ERR_Success)
            {
                return;
            }


            await scene.GetComponent<TimerComponent>().WaitAsync(3000);
            
            args.scene?.CurrentScene()?.GetComponent<AdventureComponent>()?.ShowAdventureHpBarInfo(false);
            args.scene?.CurrentScene()?.GetComponent<AdventureComponent>()?.ResetAdventure();
            
            await ETTask.CompletedTask;
        }
    }
}