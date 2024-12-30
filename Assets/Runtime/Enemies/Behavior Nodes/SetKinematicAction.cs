using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Kinematic", story: "[Self] sets Kinematic [true]", category: "Action/Physics", id: "3e8090e44a0afd2251d39b3970b7d7b0")]
public partial class SetKinematicAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<bool> True;

    protected override Status OnStart()
    {
        Self.Value.gameObject.GetComponent<Rigidbody>().isKinematic = True.Value;
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

