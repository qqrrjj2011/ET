using System;

namespace ET.Server
{
    [FriendOf(typeof(Production))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ReceiveProductionHandler : MessageLocationHandler<Unit, C2M_ReceiveProduction, M2C_ReceiveProduction>
    {
        protected override async ETTask Run(Unit unit, C2M_ReceiveProduction request, M2C_ReceiveProduction response)
        {
            ForgeComponent forgeComponent = unit.GetComponent<ForgeComponent>();
            ServerBagComponent bagComponent = unit.GetComponent<ServerBagComponent>();

            if (bagComponent.IsMaxLoad())
            {
                response.Error = ErrorCode.ERR_BagMaxLoad;
             //   reply();
                return;
            }

            if (!forgeComponent.IsExistProductionOverQueue(request.ProducitonId))
            {
                response.Error = ErrorCode.ERR_NoMakeQueueOver;
            //    reply();
                return;
            }

            Production production = forgeComponent.GetProductionById(request.ProducitonId);

            var config = ForgeProductionConfigCategory.Instance.Get(production.ConfigId);
            if (!BagHelper.AddItemByConfigId(unit, config.ItemConfigId))
            {
                response.Error = ErrorCode.ERR_AddBagItemError;
             //   reply();
                return;
            } 
            EventSystem.Instance.Publish(unit.Root(),new ET.EventType.MakeProdutionOver(){Unit = unit,ProductionConfigId = production.ConfigId});
            production.Reset();
            
            response.ProductionProto = production.ToMessage();
         //   reply();

            await ETTask.CompletedTask;
        }
    }
}