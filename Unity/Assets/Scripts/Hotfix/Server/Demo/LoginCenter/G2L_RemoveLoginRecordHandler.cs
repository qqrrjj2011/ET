using System;

namespace ET.Server
{
    [MessageHandler(SceneType.LoginCenter)]
    public class G2L_RemoveLoginRecordHandler : MessageHandler<Scene,G2L_RemoveLoginRecord,L2G_RemoveLoginRecord>
    {
        protected override async ETTask Run(Scene scene, G2L_RemoveLoginRecord request, L2G_RemoveLoginRecord response)
        {
            string account = request.Account;
            using (await scene.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.LoginCenterLock, account.GetHashCode()))
            {
                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(account);
                if (request.ServerId == zone)
                {
                    scene.GetComponent<LoginInfoRecordComponent>().Remove(account);
                }
            }
            //reply();
        }
    }
}