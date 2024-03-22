using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;

/// <summary>
/// 流程更新完毕
/// </summary>
internal class FsmUpdaterDone : IStateNode
{
    void IStateNode.OnCreate(StateMachine machine)
    {
    }
    void IStateNode.OnEnter()
    {
        Debug.Log(">>>>>>FsmUpdaterDone down finish");
        UserEventDefine.UpdateFinish.SendEventMessage();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
        
    }
}