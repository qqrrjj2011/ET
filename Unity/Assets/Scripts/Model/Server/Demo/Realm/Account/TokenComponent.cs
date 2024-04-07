using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class TokenComponent:Entity,IAwake
    {
        /// <summary>
        /// accountId - token
        /// </summary>
        public readonly Dictionary<long, string> TokenDictionary = new Dictionary<long, string>();
    }
    
}

