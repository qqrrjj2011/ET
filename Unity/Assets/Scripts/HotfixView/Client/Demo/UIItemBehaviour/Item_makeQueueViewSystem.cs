
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[EntitySystemOf(typeof(Scroll_Item_makeQueue))]
	public static partial class Scroll_Item_makeQueueSystem 
	{
		[EntitySystem]
		private static void Awake(this Scroll_Item_makeQueue self )
		{
		}

		[EntitySystem]
		private static void Destroy(this Scroll_Item_makeQueue self )
		{
			self.DestroyWidget();
		}
	}
}
