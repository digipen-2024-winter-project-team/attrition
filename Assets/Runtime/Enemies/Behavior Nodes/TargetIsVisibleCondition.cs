using Attrition.CombatTargeting;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "TargetIsVisible", story: "[TargetDetection] [Can] see its Target", category: "Conditions", id: "18533e9f1936ab0fdfbfaa1cbea61cf1")]
public partial class TargetIsVisibleCondition : Condition
{
    [SerializeReference] public BlackboardVariable<TargetDetection> TargetDetection;
    [SerializeReference] public BlackboardVariable<CanOption> Can;
    
    public enum CanOption
    {
        Can,
        Cannot,
    }
    
    public override bool IsTrue()
    {
        bool visible = TargetDetection.Value.TargetVisible;

        return Can.Value switch
        {
            CanOption.Can => visible,
            CanOption.Cannot => !visible,
            _ => visible,
        };
    }
}
