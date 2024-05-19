using ET.EventType;

namespace ET.Client
{
    [NumericWatcher(SceneType.Current,NumericType.BattleRandomSeed)]
    public class NumericWatcher_BattleRandomSeed : INumericWatcher
    {
        public void Run(Unit unit,NumbericChange args)
        {
            if (args.Unit == null)
            {
                return;
            }
            unit = args.Unit as Unit;
       
            unit.Root().CurrentScene().GetComponent<AdventureComponent>()?.SetBattleRandomSeed();
        }
 
    }
}