using System;

namespace ET.Client
{
    public static class AdventureHelper
    {
        public static async ETTask<int> RequestStartGameLevel(Scene zoneScene, int levelId)
        {
            M2C_StartGameLevel m2CStartGameLvel = null;
            try
            {
                C2M_StartGameLevel c2MStartGameLevel = C2M_StartGameLevel.Create();
                c2MStartGameLevel.LevelId = levelId;
                m2CStartGameLvel  =  (M2C_StartGameLevel) await zoneScene.GetComponent<ClientSenderComponent>().Call(c2MStartGameLevel);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (m2CStartGameLvel.Error != ErrorCode.ERR_Success)
            {
                Log.Error(m2CStartGameLvel.Error.ToString());
                return m2CStartGameLvel.Error;
            }
            return ErrorCode.ERR_Success;
        }
        
        public static async ETTask<int> RequestEndGameLevel(Scene zoneScene,BattleRoundResult battleRoundResult,int round)
        {
            M2C_EndGameLevel m2CEndGameLevel = null;
            try
            {
                C2M_EndGameLevel c2MEndGameLevel = C2M_EndGameLevel.Create();
                c2MEndGameLevel.BattleResult = (int)battleRoundResult;
                c2MEndGameLevel.Round = round;
                m2CEndGameLevel  =  (M2C_EndGameLevel) await zoneScene.GetComponent<ClientSenderComponent>().Call(c2MEndGameLevel);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (m2CEndGameLevel.Error != ErrorCode.ERR_Success)
            {
                Log.Error(m2CEndGameLevel.Error.ToString());
                return m2CEndGameLevel.Error;
            }
            
            return ErrorCode.ERR_Success;
        }
        

    }
}