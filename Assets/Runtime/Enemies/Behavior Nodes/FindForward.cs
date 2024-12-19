using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindForward", story: "[Agent] sets [forward]", category: "Action/Blackboard", id: "91d53f3ab4694c6e9fcf6b912605936d")]
public partial class FindForwardAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> agent;
    [SerializeReference] public BlackboardVariable<Vector3> forward;
    private float timer;
    [SerializeReference] public BlackboardVariable<float> delay;
    protected override Status OnStart()
    {
        forward.Value = GameObject.transform.forward;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= delay.Value)
        {
            forward.Value = GameObject.transform.forward;
            timer = 0;
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

