using System;

namespace ET.Client
{
    public static partial class ES_AttributeItemSystem
    {
        public static void Refresh(this ES_AttributeItem self, int numbericType)
        {
            self.EAttributeValueText.text = UnitHelper.GetMyUnitFromCurrentScene(self.Root().CurrentScene())
                                                     .GetComponent<NumericComponent>().GetAsLong(numbericType).ToString();
        }
        
        public static void RegisterEvent(this ES_AttributeItem self, int numbericType)
        {
            self.E_AddButton.AddListenerAsync(() => { return self.RequestAddAttribute(numbericType);});
        }
        
        public static async ETTask RequestAddAttribute(this ES_AttributeItem self, int numericType)
        {
            try
            {
                int errorCode = await NumericHelper.ReqeustAddAttributePoint(self.Root(), numericType);
                if (errorCode != ErrorCode.ERR_Success)
                {
                    return;
                }
                Log.Debug("加点成功");
                //111 self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgRoleInfo>()?.Refresh();
                EventSystem.Instance.PublishAsync(self.Root(),new EventClientType.RefreshRoleInfo(){}).Coroutine();
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
            }
            
            await ETTask.CompletedTask;
        }
        
    }
}