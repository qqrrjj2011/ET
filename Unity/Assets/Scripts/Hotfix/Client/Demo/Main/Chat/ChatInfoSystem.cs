namespace ET.Client
{
    [FriendOf(typeof(ChatInfo))]
    public class ChatInfoDestroySystem: DestroySystem<ChatInfo>
    {
        protected override void Destroy(ChatInfo self)
        {
            self.Message = null;
            self.Name    = null;
        }
    }
}