namespace ET.Client
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgBag :Entity,IAwake,IUILogic
	{

		public DlgBagViewComponent View { get => this.GetComponent<DlgBagViewComponent>();} 

		 

	}
}
