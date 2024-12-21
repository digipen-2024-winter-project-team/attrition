using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Attack Force", story: "Set force [rotation] by [power]", category: "Action/Physics", id: "0cb81013c8d434fda17d48d195d97b25")]
public partial class SetAttackForceAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> Rotation;
    [SerializeReference] public BlackboardVariable<float> Power;

    protected override Status OnStart()
    {
        Rotation.Value = new Vector3(Rotation.Value.x * Power, Rotation.Value.y, Rotation.Value.z * Power);
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

