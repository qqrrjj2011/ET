using System;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_StartGameLvelHandler : MessageLocationHandler<Unit,C2M_StartGameLevel,M2C_StartGameLevel>
    {
        protected override async ETTask Run(Unit unit, C2M_StartGameLevel request, M2C_StartGameLevel response)
        {
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

            if ( numericComponent.GetAsInt(NumericType.AdventureState) != 0 )
            {
                response.Error = ErrorCode.ERR_AlreadyAdventureState;
                //reply();
                return;
            }
            
            if ( numericComponent.GetAsInt(NumericType.DyingState) != 0 )
            {
                response.Error = ErrorCode.ERR_AdventureInDying;
               // reply();
                return;
            }
            
            if ( !BattleLevelConfigCategory.Instance.Contain(request.LevelId) )
            {
                response.Error = ErrorCode.ERR_AdventureErrorLevel;
               // reply();
                return;
            }


            BattleLevelConfig config = BattleLevelConfigCategory.Instance.Get(request.LevelId);
            if (numericComponent[NumericType.Level] < config.MiniEnterLevel[0])
            {
                response.Error = ErrorCode.ERR_AdventureLevelNotEnough;
              //  reply();
                return;
            }
            
            //设置本次战斗的随机种子，保证客户端的战斗中的每次随机产生的数能在服务器端复现
            numericComponent.Set(NumericType.BattleRandomSeed,RandomGenerator.RandUInt32());
            
            numericComponent.Set(NumericType.AdventureState, request.LevelId);
            numericComponent.Set(NumericType.AdventureStartTime,TimeInfo.Instance.ServerNow());
            
            
            //reply();
            await ETTask.CompletedTask;
        }
    }
}