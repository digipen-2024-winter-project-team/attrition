using Attrition.DamageSystem;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Simple Damage Source Set Active", story: "[SimpleDamageSource] Set [Active]", category: "Action/Damage", id: "317b0bc73ea53dc75d0791f07ef0e4d8")]
public partial class SimpleDamageSourceSetActiveAction : Action
{
    [SerializeReference] public BlackboardVariable<SimpleDamageSource> SimpleDamageSource;
    [SerializeReference] public BlackboardVariable<bool> Active;

    protected override Status OnStart()
    {
        SimpleDamageSource.Value.SetActive(Active.Value);
        
        return Status.Success;
    }
}

