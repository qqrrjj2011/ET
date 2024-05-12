using System.Collections.Generic;


namespace ET.Server
{
	public static partial class RealmGateAddressHelper
	{
		public static StartSceneConfig GetGate(int zone, string account)
		{
			ulong hash = (ulong)account.GetLongHashCode();
			int gateZone = zone % 1000 + 1;
			List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[gateZone];
			
			return zoneGates[(int)(hash % (ulong)zoneGates.Count)];
		}
	}
}
