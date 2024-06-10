
using System;

namespace ET.Client
{
    public static class ForgeHelper
    {
        //请求开始生产物品
        public static async ETTask<int> StartProduction(Scene ZoneScene, int productionConfigId)
        {
            //判定生成配方是否存在
            if (!ForgeProductionConfigCategory.Instance.Contain(productionConfigId))
            {
                return ErrorCode.ERR_MakeConfigNotExist;
            }

            //判定生产材料数量是否满足
            var config        = ForgeProductionConfigCategory.Instance.Get(productionConfigId);
            int materailCount = UnitHelper.GetMyUnitNumericComponent(ZoneScene.CurrentScene()).GetAsInt(config.ConsumId);
            if (materailCount<config.ConsumeCount)
            {
                return ErrorCode.ERR_MakeConsumeError;
            }
            
            M2C_StartProduction m2CStartProduction = null;

            try
            {
                C2M_StartProduction c2MStartProduction = C2M_StartProduction.Create();
                c2MStartProduction.ConfigId = productionConfigId;
                m2CStartProduction = (M2C_StartProduction) await ZoneScene.GetComponent<ClientSenderComponent>().Call(c2MStartProduction);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (m2CStartProduction.Error != ErrorCode.ERR_Success)
            {
                return m2CStartProduction.Error;
            }
            
            ZoneScene.GetComponent<ForgeComponent>().AddOrUpdateProductionQueue(m2CStartProduction.ProductionProto);
            return ErrorCode.ERR_Success;
        }


        //请求获取生产好的物品
        public static async ETTask<int> ReceivedProductionItem(Scene ZoneScene, long productionId)
        {
            //背包已满
            if (ZoneScene.GetComponent<BagComponent>().IsMaxLoad())
            {
                return ErrorCode.ERR_BagMaxLoad;
            }
            
            M2C_ReceiveProduction m2CReciveProduction = null;

            try
            {
                C2M_ReceiveProduction c2MReceiveProduction = C2M_ReceiveProduction.Create();
                c2MReceiveProduction.ProducitonId = productionId;
                m2CReciveProduction = (M2C_ReceiveProduction) await ZoneScene.GetComponent<ClientSenderComponent>().Call(c2MReceiveProduction);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (m2CReciveProduction.Error != ErrorCode.ERR_Success)
            {
                return m2CReciveProduction.Error;
            }
            
            ZoneScene.GetComponent<ForgeComponent>().AddOrUpdateProductionQueue(m2CReciveProduction.ProductionProto);
            return ErrorCode.ERR_Success;

        }
        
        
    }
}