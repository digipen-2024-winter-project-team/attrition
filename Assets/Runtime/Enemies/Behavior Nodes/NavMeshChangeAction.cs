using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "NavMesh Change", story: "[Self] set NavMesh [Enabled]", category: "Action/GameObject", id: "b093573a6960244776bb334492dd66d6")]
public partial class NavMeshChangeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<bool> Enabled;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Self.Value.gameObject.GetComponent<NavMeshAgent>().enabled = Enabled.Value;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

