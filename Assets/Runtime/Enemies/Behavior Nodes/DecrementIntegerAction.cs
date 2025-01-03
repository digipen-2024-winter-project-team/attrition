using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Decrement Integer", story: "Decrement [Integer]", category: "Action/Debug", id: "7ddcdc33550eb5ca2ad9fc0480acf7b4")]
public partial class DecrementIntegerAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Integer;

    protected override Status OnStart()
    {
        Integer.Value--;
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

