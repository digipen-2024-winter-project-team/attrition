using Attrition.DamageSystem;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetDurabilityInvincibility", story: "Set [Durability] Invincibility [True]", category: "Action/Damage", id: "a865dd472b4da69dc4886d5476449324")]
public partial class SetDurabilityInvincibilityAction : Action
{
    [SerializeReference] public BlackboardVariable<Durability> Durability;
    [SerializeReference] public BlackboardVariable<bool> True;

    protected override Status OnStart()
    {
        if (Durability.Value == null)
        {
            return Status.Failure;
        }
        
        Durability.Value.SetInvincible(True.Value);
        
        return Status.Success;
    }
}

