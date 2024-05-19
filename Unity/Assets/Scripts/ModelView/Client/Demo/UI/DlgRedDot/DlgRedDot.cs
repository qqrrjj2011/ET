namespace ET.Client
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgRedDot :Entity,IAwake,IUILogic
	{

		public DlgRedDotViewComponent View { get => this.GetComponent<DlgRedDotViewComponent>();} 

		public int RedDotBagCount1 = 0;
		public int RedDotBagCount2 = 0;
		public int RedDotMailCount1 = 0;
		public int RedDotMailCount2 = 0;

	}
}
