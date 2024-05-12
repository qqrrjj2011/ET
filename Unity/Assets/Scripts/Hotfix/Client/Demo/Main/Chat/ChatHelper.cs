using System;

namespace ET.Client
{
    public static class ChatHelper
    {
        public static async ETTask<int> SendMessage(Scene ZoneScene, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return ErrorCode.ERR_ChatMessageEmpty;
            }
            
            Chat2C_SendChatInfo chat2CSendChatInfo = null;
            try
            {
                C2Chat_SendChatInfo c2ChatSendChatInfo = C2Chat_SendChatInfo.Create();
                c2ChatSendChatInfo.ChatMessage = message;
                chat2CSendChatInfo = (Chat2C_SendChatInfo) await ZoneScene.GetComponent<ClientSenderComponent>().Call(c2ChatSendChatInfo);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (chat2CSendChatInfo.Error != ErrorCode.ERR_Success)
            {
                return chat2CSendChatInfo.Error;
            }
            return ErrorCode.ERR_Success;
        }
    }
}