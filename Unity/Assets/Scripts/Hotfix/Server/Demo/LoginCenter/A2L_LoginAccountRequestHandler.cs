namespace ET.Server
{
    [MessageHandler(SceneType.LoginCenter)]
    public class A2L_LoginAccountRequestHandler:MessageHandler<Scene,A2L_LoginAccountRequest,L2A_LoginAccountResponse>
    {
        protected override async ETTask Run(Scene scene, A2L_LoginAccountRequest request, L2A_LoginAccountResponse response)
        {
            using (await scene.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.LoginCenterLock,request.Account.GetHashCode()))
            {
                if (!scene.GetComponent<LoginInfoRecordComponent>().IsExist(request.Account))
                {
                    return;
                }

                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(request.Account);
                StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(zone,request.Account);
                
                L2G_DisconnectGateRequest l2GDisconnectGateRequest = L2G_DisconnectGateRequest.Create();
                l2GDisconnectGateRequest.Account = request.Account;
                var g2LDisconnectGateResponse = (G2L_DisconnectGateResponse) await scene.GetComponent<MessageSender>().Call(gateConfig.ActorId, l2GDisconnectGateRequest);

                response.Error = g2LDisconnectGateResponse.Error;
            }
            
            
            await ETTask.CompletedTask;
        }
    }
    
}

