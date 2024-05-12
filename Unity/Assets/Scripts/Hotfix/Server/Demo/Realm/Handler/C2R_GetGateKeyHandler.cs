namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    public class C2R_GetGateKeyHandler:MessageSessionHandler<C2R_GetGateKey,R2C_GetGateKey>
    {
        protected override async ETTask Run(Session session, C2R_GetGateKey request, R2C_GetGateKey response)
        {
            // 随机分配一个Gate
            StartSceneConfig config = RealmGateAddressHelper.GetGate(session.Zone(), request.Account);
            Log.Debug($"gate address: {config}");
			
            // 向gate请求一个key,客户端可以拿着这个key连接gate
            R2G_GetLoginKey r2GGetLoginKey = R2G_GetLoginKey.Create();
            r2GGetLoginKey.Account = request.Account;
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey) await session.Fiber().Root.GetComponent<MessageSender>().Call(
                config.ActorId, r2GGetLoginKey);
            
            if (g2RGetLoginKey.Error != ErrorCode.ERR_Success)
            {
                Log.Error(">>>>>>>g2RGetLoginKey Error:"+g2RGetLoginKey.Error);                
                return;
            }

            response.GateAddress = config.InnerIPPort.ToString();
            response.GateSessionTokey = g2RGetLoginKey.GateToken;
            response.GateId = g2RGetLoginKey.GateId;

      //      Log.Info(">>>>>>>>>>>response.Address:"+response.Address  + "response.Key:"+response.Key);
			
            CloseSession(session).Coroutine();
            await ETTask.CompletedTask;
        }
        
        private async ETTask CloseSession(Session session)
        {
            await session.Root().GetComponent<TimerComponent>().WaitAsync(1000);
            session.Dispose();
        }
    }
    
}
