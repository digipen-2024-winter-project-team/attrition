using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetToGameObjectPosition", story: "Set [Position] to [GameObject] Position", category: "Action/GameObject", id: "a4cebfc60404a1f8e8d7d4c20c8906b0")]
public partial class SetToGameObjectPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> Position;
    [SerializeReference] public BlackboardVariable<GameObject> GameObject;

    protected override Status OnStart()
    {
        if (GameObject.Value == null) return Status.Failure;

        Position.Value = GameObject.Value.transform.position;
        
        return Status.Success;
    }
}

