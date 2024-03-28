using System;
using System.Collections.Generic;
using UnityEditor;

namespace ET
{
    [CustomEditor(typeof(GlobalConfig))]
    public class GlobalConfigEditor : Editor
    {
        private CodeMode codeMode;
        private BuildType buildType;
        private AppRunType appRunType;
        

        private void OnEnable()
        {
            GlobalConfig globalConfig = (GlobalConfig)this.target;
            this.codeMode = globalConfig.CodeMode;
            globalConfig.BuildType = EditorUserBuildSettings.development ? BuildType.Debug : BuildType.Release;
            this.buildType = globalConfig.BuildType;
            this.appRunType = globalConfig.AppRunType;

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GlobalConfig globalConfig = (GlobalConfig)this.target;

            if (this.codeMode != globalConfig.CodeMode)
            {
                this.codeMode = globalConfig.CodeMode;
                this.serializedObject.Update();
                AssemblyTool.DoCompile();
            }

            if (this.buildType != globalConfig.BuildType)
            {
                this.buildType = globalConfig.BuildType;
                EditorUserBuildSettings.development = this.buildType switch
                {
                    BuildType.Debug => true,
                    BuildType.Release => false,
                    _ => throw new ArgumentOutOfRangeException()
                };
                this.serializedObject.Update();
                AssemblyTool.DoCompile();
            }

            if (this.appRunType != globalConfig.AppRunType)
            {
                // 获取当前Standalone平台的预处理器符号
                string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
 
                // 删除指定的宏定义
                string[] defines = currentDefines.Split(';');
                List<string> updatedDefines = new List<string>();
                string curAppRuntypeDefine = appRunType.ToString();
                foreach (string define in defines)
                {
                    if (!string.IsNullOrEmpty(define))
                    {
                        updatedDefines.Add(define);
                    }
                }

                if (updatedDefines.Contains(curAppRuntypeDefine))
                {
                    updatedDefines.Remove(curAppRuntypeDefine);
                }

                this.appRunType = globalConfig.AppRunType;
                curAppRuntypeDefine = this.appRunType.ToString();
                updatedDefines.Add(curAppRuntypeDefine);

                // 将剩余的定义重新组合成字符串
                string newDefines = string.Join(";", updatedDefines.ToArray());

                // 设置新的预处理器符号
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    BuildTargetGroup.Standalone,
                    newDefines
                );
            }
        }
    }
}