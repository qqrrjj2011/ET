using System;

namespace ET
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class EntitySystemAttribute: BaseAttribute
	{
	}
	
	/// <summary>
	/// 为了兼容EUI
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ObjectSystemAttribute: BaseAttribute
	{
	}
}