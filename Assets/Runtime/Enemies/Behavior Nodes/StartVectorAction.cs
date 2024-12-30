using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StartVector", story: "Set [StartLocation] of [Self]", category: "Action/Navigation", id: "4210b4a52b828af8b1c8b9ffd19c635c")]
public partial class StartVectorAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> StartLocation;
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    protected override Status OnStart()
    {
        StartLocation.Value = Self.Value.gameObject.transform.position;
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

