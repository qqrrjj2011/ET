using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class TokenComponent:Entity,IAwake
    {
        /// <summary>
        /// account - token
        /// </summary>
        public readonly Dictionary<string, string> TokenDictionary = new Dictionary<string, string>();
    }
    
}

