using Attrition.CombatTargeting;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "TargetExists", story: "[TargetDetection] [Has] [Target]", category: "Conditions", id: "013d8b0380100971ba2d5360b138e210")]
public partial class TargetExistsCondition : Condition
{
    [SerializeReference] public BlackboardVariable<TargetDetection> TargetDetection;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<ExistOption> Has; 

    public enum ExistOption
    {
        Has,
        [InspectorName("Doesn't Have")]
        DoesntHave,
    }
        
    public override bool IsTrue()
    {
        bool hasTarget = TargetDetection.Value.Target != null;

        Target.Value = hasTarget 
            ? TargetDetection.Value.Target.gameObject 
            : null;
        
        return Has.Value switch
        {
            ExistOption.Has => hasTarget,
            ExistOption.DoesntHave => !hasTarget,
            _ => hasTarget
        };
    }
}
