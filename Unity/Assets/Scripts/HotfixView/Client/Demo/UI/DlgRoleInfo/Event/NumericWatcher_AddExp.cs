using System;
using ET.EventType;

namespace ET.Client
{
    [NumericWatcher(SceneType.Current,NumericType.Exp)]
    public class NumericWatcher_AddExp : INumericWatcher
    {
        public void Run(Unit unit,NumbericChange args)
        {
            // if (args.Parent == null)
            // {
            //     return;
            // }
            // unit = args.Parent as Unit;
            
            if (args.NumericType == NumericType.Exp)
            {
                NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

                int unitLevel = numericComponent.GetAsInt(NumericType.Level);

                if (PlayerLevelConfigCategory.Instance.Contain(unitLevel))
                {
                    long needExp  = PlayerLevelConfigCategory.Instance.Get(unitLevel).NeedExp;
                
                    if (args.New >= needExp)
                    {
                        RedDotHelper.ShowRedDotNode(unit.Root(), "UpLevelButton");
                    }
                    else
                    {
                        if (RedDotHelper.IsLogicAlreadyShow(unit.Root(), "UpLevelButton"))
                        {
                            RedDotHelper.HideRedDotNode(unit.Root(), "UpLevelButton");
                        }
                    }
                }
            }     
            
           //111 unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgRoleInfo>()?.Refresh();
           EventSystem.Instance.PublishAsync(unit.Root(),new EventClientType.RefreshRoleInfo(){}).Coroutine();

        }

        
    }
    
}