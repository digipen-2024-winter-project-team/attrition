using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomPosition", story: "Find Random Navigable [Position] in [Radius] from [StartPosition]", category: "Action/Navigation", id: "a4697ea2c9ee911b46b8404d34ad0c23")]
public partial class GetRandomPositionAction : Action {
    
    [SerializeReference] public BlackboardVariable<Vector3> Position;
    [SerializeReference] public BlackboardVariable<float> Radius;
    [SerializeReference] public BlackboardVariable<Vector3> StartPosition;
    [SerializeReference] public BlackboardVariable<int> MaxPositionAttempts = new BlackboardVariable<int>(10);
    
    protected override Status OnStart()
    {
        for (int attempt = 0; attempt < MaxPositionAttempts.Value; attempt++)
        {
            Vector3 randomPosition = StartPosition.Value + Random.insideUnitSphere * Radius.Value * 2f;
            
            if (NavMesh.SamplePosition(randomPosition, out var hit, 1f, NavMesh.AllAreas))
            {
                Position.Value = hit.position;
                return Status.Success;
            }
        }
        
        return Status.Failure;
    }
}

