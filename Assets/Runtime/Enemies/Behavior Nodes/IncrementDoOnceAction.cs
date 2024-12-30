using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IncrementDoOnce", story: "Increment [Integer]", category: "Action/Delay", id: "d0c6a1c367da1891191c4b7c7e0c49f4")]
public partial class IncrementDoOnceAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Integer;

    protected override Status OnStart()
    {
        Integer.Value++;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

