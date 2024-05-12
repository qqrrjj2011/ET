namespace ET.Server
{
	[ComponentOf(typeof(Player))]
	public class PlayerSessionComponent : Entity, IAwake
	{
		private EntityRef<Session> session;

		public Session gateSession
		{
			get
			{
				return this.session;
			}
			set
			{
				this.session = value;
			}
		}
	}
}