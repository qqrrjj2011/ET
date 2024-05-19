namespace ET
{
    namespace EventType
    {
        public struct SceneChangeStart
        {
        }
        
        public struct SceneChangeFinish
        {
            public Scene ZoneScene;
            public Scene CurrentScene;
        }
        
        public struct AfterCreateClientScene
        {
        }
        
        public struct AfterCreateCurrentScene
        {
        }

        public struct AppStartInitFinish
        {
        }

        public struct LoginFinish
        {
        }

        public struct EnterMapFinish
        {
        }

        public struct AfterUnitCreate
        {
            public Unit Unit;
        }
        
        public struct UpdateChatInfo
        {
            public Scene ZoneScene;
        }
        public struct BattleWin
        {
            public Unit Unit;
            public int  LevelId;
        }
        
        public struct MakeProdutionOver
        {
            public Unit Unit;
            public int ProductionConfigId;
        }
        
        public struct UpdateTaskInfo
        {
            public Scene ZoneScene;
        }
        
        public struct AdventureBattleOver
        {
            public Scene scene;
            public Unit WinUnit;
        }
        
        public struct AdventureBattleReport
        {
            public Scene scene;
            public BattleRoundResult BattleRoundResult;
            public int Round;
        }
        
        public struct AdventureRoundReset
        {
            public Scene ZoneScene;
        }
        
        public struct AdventureBattleRound
        {
            public Scene scene;
            public Unit AttackUnit;
            public Unit TargetUnit;
        }
        
        public struct AdventureBattleRoundView
        {
            public Scene scene;
            public Unit AttackUnit;
            public Unit TargetUnit;
        }
        
        
        public struct ShowDamageValueView
        {
            public Scene ZoneScene;
            public Unit TargetUnit;
            public long DamamgeValue;
        }
        
        public struct ShowAdventureHpBar
        {
            [StaticField]
            public static  ShowAdventureHpBar Instance = new ShowAdventureHpBar();
            public Unit Unit;
            public bool isShow;
        }
        
        public struct MakeQueueOver
        {
            [StaticField]
            public static readonly MakeQueueOver Instance = new MakeQueueOver();
        }
      
    }
}