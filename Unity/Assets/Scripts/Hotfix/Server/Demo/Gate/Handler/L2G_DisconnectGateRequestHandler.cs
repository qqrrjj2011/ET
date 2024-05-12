namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    [FriendOf(typeof(SessionPlayerComponent))]
    public class L2G_DisconnectGateRequestHandler : MessageHandler<Scene, L2G_DisconnectGateRequest, G2L_DisconnectGateResponse>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateRequest request, G2L_DisconnectGateResponse response)
        {
            
            using (await scene.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.LoginGate, request.Account.GetHashCode()))
            {
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player player = playerComponent.Get(request.Account);

                if (player == null)
                {
                    return;
                }

                scene.GetComponent<GateSessionKeyComponent>().Remove(request.Account);
               //111 Session gateSession = player.ClientSession;
               Session gateSession = player.GetComponent<PlayerSessionComponent>().gateSession;
                if (gateSession != null && !gateSession.IsDisposed)
                {
                    if (gateSession.GetComponent<SessionPlayerComponent>() != null)
                    {
                        gateSession.GetComponent<SessionPlayerComponent>().isLoginAgain = true;
                    }

                    A2C_Disconnect a2CDisconnect = A2C_Disconnect.Create();
                    a2CDisconnect.Error = ErrorCode.ERR_OtherAccountLogin;
                    gateSession.Send(a2CDisconnect);
                    gateSession?.Disconnect().Coroutine();
                }
                player.AddComponent<PlayerOfflineOutTimeComponent>();
            }
        }
    }
}

