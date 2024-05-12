using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class GateSessionKeyComponent : Entity, IAwake
    {
        //account - tokenkey
        public readonly Dictionary<string,string> sessionKey = new();
    }
}