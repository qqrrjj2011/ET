using ET.EventType;

namespace ET.Server
{

    [NumericWatcher(SceneType.Map,NumericType.Level)]
    public class NumericWatcher_UpLevel : INumericWatcher
    {
        public void Run(Unit unit,NumbericChange args)
        {
            // if (args.Parent == null)
            // {
            //     return;
            // }
            // unit = args.Parent as Unit;
            unit.GetComponent<ServerTasksComponent>().TriggerTaskAction(TaskActionType.UpLevel,(int)args.New);
            
            RankHelper.AddOrUpdateLevelRank(unit);
        }
    }
}