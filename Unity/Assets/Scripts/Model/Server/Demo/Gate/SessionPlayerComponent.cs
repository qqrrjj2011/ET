namespace ET.Server
{
	[ComponentOf(typeof(Session))]
	public class SessionPlayerComponent : Entity, IAwake, IDestroy
	{
		private EntityRef<Player> player;
		
		// public long PlayerId;
		// public long PlayerInstanceId;
		// public string Account;
		public bool isLoginAgain = false;

		public Player Player
		{
			get
			{
				return this.player;
			}
			set
			{
				this.player = value;
			}
		}
	}
}