using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set NavMeshAgent Speed", story: "Set NavMeshAgent speed to [Speed]", category: "Action", id: "c02e4fe29a0f1fd4dbace08842a8276e")]
public partial class SetNavMeshAgentSpeedAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Speed;

    private NavMeshAgent agent;
    
    protected override Status OnStart()
    {
        if (agent == null)
        {
            agent = GameObject.GetComponentInChildren<NavMeshAgent>();

            if (agent == null) return Status.Failure;
        }
        
        agent.speed = Speed.Value;
        
        return Status.Success;
    }
}

