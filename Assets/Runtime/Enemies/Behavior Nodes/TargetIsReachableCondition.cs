using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "TargetIsReachable", story: "[Target] [Is] Reachable", category: "Conditions", id: "f3494fa725ebde0c75ebce52f7844226")]
public partial class TargetIsReachableCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<IsOption> Is;
    
    public enum IsOption
    {
        Is,
        Isnt,
    }
    
    private NavMeshAgent agent;
    
    public override bool IsTrue()
    {
        if (agent == null)
        {
            agent = GameObject.GetComponentInChildren<NavMeshAgent>();
        }

        var path = new NavMeshPath();
        bool reachable = agent != null
                         && Target.Value != null
                         && agent.CalculatePath(Target.Value.transform.position, path)
                         && path.status == NavMeshPathStatus.PathComplete; 
        
        return Is.Value switch
        {
            IsOption.Is => reachable,
            IsOption.Isnt => !reachable,
            _ => reachable,
        };
    }
}
