namespace ET.Server
{
    [MessageHandler(SceneType.LoginCenter)]
    public class A2L_LoginAccountRequestHandler:MessageHandler<Scene,A2L_LoginAccountRequest,L2A_LoginAccountResponse>
    {
        protected override async ETTask Run(Scene scene, A2L_LoginAccountRequest request, L2A_LoginAccountResponse response)
        {
            long accountId = request.AccountId;

            using (await scene.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.LoginCenterLock,accountId.GetHashCode()))
            {
                if (!scene.GetComponent<LoginInfoRecordComponent>().IsExist(accountId))
                {
                    return;
                }

                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(accountId);
                StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(zone,accountId.ToString());
                
                L2G_DisconnectGateRequest l2GDisconnectGateRequest = L2G_DisconnectGateRequest.Create();
                l2GDisconnectGateRequest.AccountId = accountId;
                var g2LDisconnectGateResponse = (G2L_DisconnectGateResponse) await scene.GetComponent<MessageSender>().Call(gateConfig.ActorId, l2GDisconnectGateRequest);

                response.Error = g2LDisconnectGateResponse.Error;
            }
            
            
            await ETTask.CompletedTask;
        }
    }
    
}

