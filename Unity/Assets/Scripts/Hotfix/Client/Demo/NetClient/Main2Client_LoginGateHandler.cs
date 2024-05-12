namespace ET.Client
{
    [MessageHandler(SceneType.NetClient)]
    public class Main2Client_LoginGateHandler:MessageHandler<Scene,Main2NetClient_LoginGate,NetClient2Main_LoginGate>
    {
        protected override async ETTask Run(Scene root, Main2NetClient_LoginGate request, NetClient2Main_LoginGate response)
        {
            NetComponent netComponent = root.GetComponent<NetComponent>();
           // 创建一个gate Session,并且保存到SessionComponent中
            Session gateSession = await netComponent.CreateRouterSession(NetworkHelper.ToIPEndPoint(request.GateAddress), request.Account, "");
            gateSession.AddComponent<ClientSessionErrorComponent>();
            root.GetComponent<SessionComponent>().Session = gateSession;
            
            
            C2G_LoginGate c2GLoginGate = C2G_LoginGate.Create();
            c2GLoginGate.GateTokenKey = request.GateToken;
            c2GLoginGate.RoleId = request.RoleId;
           // c2GLoginGate.GateId = response.g;
            G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(c2GLoginGate);
            if (g2CLoginGate.Error != ErrorCode.ERR_Success)
            {
                Log.Error(">>>>>>g2CLoginGate error:"+g2CLoginGate.Error);
               return; 
            }
            Log.Debug("登陆gate成功!");
            
            response.PlayerId = g2CLoginGate.PlayerId;
            
            await ETTask.CompletedTask;
        }
    }
}

