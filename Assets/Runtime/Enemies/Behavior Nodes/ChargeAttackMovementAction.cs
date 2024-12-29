using System;
using System.Linq;
using Attrition.Common.Physics;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChargeAttackMovement", story: "Charge at [Target] with speed [ChargeSpeed]", category: "Action", id: "154608b92d19ac49d280c77f313543a0")]
public partial class ChargeAttackMovementAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> ChargeSpeed;
    [SerializeReference] public BlackboardVariable<float> OvershootLength;
    [SerializeReference] public BlackboardVariable<float> RaycastHeight = new(0.25f);
    
    private Vector3 targetPosition;
    private NavMeshAgent agent;
    private float ogAgentSpeed;
    
    protected override Status OnStart()
    {
        if (agent == null)
        {
            agent = GameObject.GetComponentInChildren<NavMeshAgent>();
        }

        if (agent == null)
        {
            return Status.Failure;
        }
        
        // Evaluate charge target with overshoot
        Vector3 currentTargetPosition = Target.Value.transform.position;
        Vector3 toTarget = (currentTargetPosition - GameObject.transform.position).normalized;

        targetPosition = currentTargetPosition + toTarget * OvershootLength.Value;
        
        // Evaluate charge target based on possible wall collision
        Vector3 agentCenter = agent.transform.position + Vector3.up * RaycastHeight.Value;
        Vector3 toOvershoot = targetPosition - agentCenter;
        float targetDistance = toOvershoot.magnitude;
        
        if (Physics.Raycast(agentCenter, toOvershoot, out var castHit, targetDistance, GameInfo.Ground.Mask))
        {
            targetPosition = castHit.point;
        }
        
        // Raycast to floor
        if (Physics.Raycast(targetPosition, Vector3.down, out var hit, Mathf.Infinity, GameInfo.Ground.Mask))
        {
            targetPosition = hit.point;
        }
        
        // Evaluate charge target based on navmesh
        NavMesh.SamplePosition(targetPosition, out var navHit, 5f, NavMesh.AllAreas);
        targetPosition = navHit.position;

        
        // Set agent destination and speed
        var path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);
        agent.SetDestination(path.corners.Last());
        ogAgentSpeed = agent.speed;
        agent.speed = ChargeSpeed.Value;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return !agent.pathPending && agent.remainingDistance < agent.stoppingDistance
            ? Status.Success
            : Status.Running;
    }

    protected override void OnEnd()
    {
        agent.speed = ogAgentSpeed;
    }
}

