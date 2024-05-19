using ET.EventType;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class NumericChangeEvent_NoticeToClient: AEvent<Scene,NumbericChange>
    {
        protected override  async ETTask Run(Scene scene, NumbericChange a)
        {
            NumbericChange args = (NumbericChange)a;
            if (args.Unit is not Unit unit)
            {
                return;
            }

            //只允许通知玩家Unit
            if (unit.Type() != UnitType.Player)
            {
                return;
            }

            unit.GetComponent<NumericNoticeComponent>()?.NoticeImmediately(args);
            await ETTask.CompletedTask;
        }

       
    }
}