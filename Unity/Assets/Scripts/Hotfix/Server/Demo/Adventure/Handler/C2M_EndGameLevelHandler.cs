using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_EndGameLevelHandler : MessageLocationHandler<Unit,C2M_EndGameLevel,M2C_EndGameLevel>
    {
        protected override async ETTask Run(Unit unit, C2M_EndGameLevel request, M2C_EndGameLevel response)
        {
            //检测关卡信息是否正常
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            
            int levelId = numericComponent.GetAsInt(NumericType.AdventureState);
            if ( levelId == 0 || !BattleLevelConfigCategory.Instance.Contain(levelId) )
            {
                response.Error = ErrorCode.ERR_AdventureLevelIdError;
              //  reply();
                return;
            }
            
            
            //检测上传的回合数是否正常
            if ( request.Round <= 0 )
            {
                response.Error = ErrorCode.ERR_AdventureRoundError;
              //  reply();
                return;
            }
            
            //战斗失败直接进入垂死状态
            if ( request.BattleResult == (int)BattleRoundResult.LoseBattle )
            {
                numericComponent.Set(NumericType.DyingState,1);
                numericComponent.Set(NumericType.AdventureState,0);
             //   reply();
                return;
            }
            
            
            if ( request.BattleResult != (int)BattleRoundResult.WinBattle )
            {
                response.Error = ErrorCode.ERR_AdventureResultError;
              //  reply();
                return;
            }
            
            
            //检测战斗胜利结果是否正常
            if ( !unit.GetComponent<AdventureCheckComponent>().CheckBattleWinResult(request.Round) )
            {
                response.Error = ErrorCode.ERR_AdventureWinResultError;
             //   reply();
                return;
            }

            
            numericComponent.Set(NumericType.AdventureState,0);
           // reply();

            EventSystem.Instance.Publish(unit.Root(),new ET.EventType.BattleWin(){Unit =  unit,LevelId =  levelId});
            
            
            //战斗胜利增加经验值
            numericComponent[NumericType.Exp] += BattleLevelConfigCategory.Instance.Get(levelId).RewardExp;
            
            numericComponent[NumericType.IronStone] += 3600;
            numericComponent[NumericType.Fur]       += 3600;
    
            
            
            await ETTask.CompletedTask;
        }
    }
}