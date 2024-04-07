using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class GateSessionKeyComponent : Entity, IAwake
    {
        // accountId - token
        public readonly Dictionary<long,string> sessionKey = new();
    }
}