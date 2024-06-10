using System;
using ET.EventType;

namespace ET.Client
{
    [NumericWatcher(SceneType.Current,NumericType.AttributePoint)]
    public class NumericWatcher_AddAttributePoint : INumericWatcher
    {
        public void Run(Unit unit,NumbericChange args)
        {
            // if (args.Parent == null)
            // {
            //     return;
            // }
            // unit = args.Parent as Unit;
          
            if (args.NumericType == NumericType.AttributePoint)
            {
                if (args.New > 0)
                {
                    RedDotHelper.ShowRedDotNode(unit.Root(), "AddAttribute");
                }
                else
                {
                    if (RedDotHelper.IsLogicAlreadyShow(unit.Root(), "AddAttribute"))
                    {
                        RedDotHelper.HideRedDotNode(unit.Root(), "AddAttribute");
                    }
                }
            }
            
           //111 unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgRoleInfo>()?.Refresh();
           EventSystem.Instance.PublishAsync(unit.Root(),new EventClientType.RefreshRoleInfo(){}).Coroutine();

        }

        
    }
    
}