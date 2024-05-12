namespace ET.Server
{
    public enum PlayerState
    {
        Disconnect,
        Gate,
        Game,
    }
    
    [ChildOf(typeof(PlayerComponent))]
    public sealed class Player : Entity, IAwake<string,long>
    {
        public string Account { get;  set; }
        
        /// <summary>
        /// UnitId = RoleId
        /// </summary>
        public long UnitId { get; set; }

        public PlayerState PlayerState { get; set; }

       // public Session ClientSession { get; set; }

        public ActorId ChatInfoActorId { get; set; }

        public long ChatInfoInstanceID{ get; set; }
    }
}