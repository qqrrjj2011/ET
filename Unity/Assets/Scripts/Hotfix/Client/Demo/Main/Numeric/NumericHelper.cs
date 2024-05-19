using System;

namespace ET.Client
{
    public static class NumericHelper
    {
        public static async ETTask<int> TestUpdateNumeric(Scene zoneScene)
        {
            M2C_TestUnitNumeric m2CTestUnitNumeric = null;
            try
            {
                m2CTestUnitNumeric  =  (M2C_TestUnitNumeric) await zoneScene.GetComponent<SessionComponent>().Session.Call(C2M_TestUnitNumeric.Create());
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (m2CTestUnitNumeric.Error != ErrorCode.ERR_Success)
            {
                Log.Error(m2CTestUnitNumeric.Error.ToString());
                return m2CTestUnitNumeric.Error;
            }
            return ErrorCode.ERR_Success;
        }
        
        
        public static async ETTask<int> ReqeustAddAttributePoint(Scene zoneScene,int numericType)
        {
            M2C_AddAttributePoint m2CAddAttributePoint = null;
            try
            {
                C2M_AddAttributePoint c2MAddAttributePoint = C2M_AddAttributePoint.Create();
                c2MAddAttributePoint.NumericType = numericType;
                m2CAddAttributePoint  =  (M2C_AddAttributePoint) await zoneScene.GetComponent<SessionComponent>().Session.Call(c2MAddAttributePoint);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (m2CAddAttributePoint.Error != ErrorCode.ERR_Success)
            {
                Log.Error(m2CAddAttributePoint.Error.ToString());
                return m2CAddAttributePoint.Error;
            }
            return ErrorCode.ERR_Success;
        }
        
        
        public static async ETTask<int> ReqeustUpRoleLevel(Scene zoneScene)
        {
            M2C_UpRoleLevel m2CUpRoleLevel = null;
            try
            {
                m2CUpRoleLevel  =  (M2C_UpRoleLevel) await zoneScene.GetComponent<SessionComponent>().Session.Call(C2M_UpRoleLevel.Create());
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (m2CUpRoleLevel.Error != ErrorCode.ERR_Success)
            {
                Log.Error(m2CUpRoleLevel.Error.ToString());
                return m2CUpRoleLevel.Error;
            }
            return ErrorCode.ERR_Success;
        }
        
    }
}