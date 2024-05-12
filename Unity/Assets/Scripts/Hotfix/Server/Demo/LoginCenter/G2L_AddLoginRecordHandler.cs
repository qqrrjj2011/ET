using System;

namespace ET.Server
{
    [MessageHandler(SceneType.LoginCenter)]
    public class G2L_AddLoginRecordHandler: MessageHandler<Scene, G2L_AddLoginRecord, L2G_AddLoginRecord>
    {
        protected override async ETTask Run(Scene scene, G2L_AddLoginRecord request, L2G_AddLoginRecord response)
        {
            string account = request.Account;
            using (await scene.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.LoginCenterLock, account.GetHashCode()))
            {
                scene.GetComponent<LoginInfoRecordComponent>().Remove(account);
                scene.GetComponent<LoginInfoRecordComponent>().Add(account,request.ServerId);
            }
          //  reply();
        }
    }
}