using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomLocation", story: "Find Navigable [Location] in [Radius] from [Start]", category: "Action/Navigation", id: "a4697ea2c9ee911b46b8404d34ad0c23")]
public partial class RandomLocationAction : Action {
    [SerializeReference] public BlackboardVariable<Vector3> location;
    [SerializeReference] public BlackboardVariable<float> radius;
    [SerializeReference] public BlackboardVariable<GameObject> spawn;
    protected override Status OnStart()
    {
        Vector3 randomLocation = Random.insideUnitSphere * radius.Value * 2f;
        randomLocation += spawn.Value.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomLocation, out hit, radius.Value, NavMesh.AllAreas);
        location.Value = hit.position;
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

