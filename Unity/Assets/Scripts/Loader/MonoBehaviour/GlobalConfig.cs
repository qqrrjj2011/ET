using UnityEngine;
using YooAsset;

namespace ET
{
    public enum CodeMode
    {
        Client = 1,
        Server = 2,
        ClientServer = 3,
    }
    
    public enum BuildType
    {
        Debug,
        Release,
    }

    /// <summary>
    /// 全换对应宏变量，方便程序中调用
    /// </summary>
    public enum AppRunType
    {
        Publish,
        Local
    }

    [CreateAssetMenu(menuName = "ET/CreateGlobalConfig", fileName = "GlobalConfig", order = 0)]
    public class GlobalConfig: ScriptableObject
    {
        public CodeMode CodeMode;

        public bool EnableDll;

        public BuildType BuildType;

        public AppType AppType;

        public EPlayMode EPlayMode;

        public AppRunType AppRunType;
    }
}