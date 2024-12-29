using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindPlayer", story: "Find [Player]", category: "Action/Find", id: "41925f56acf3e4ff1f9ae556d9635d53")]
public partial class FindPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;

    protected override Status OnStart()
    {
        Player.Value = GameObject.FindWithTag("Player");
        return Status.Success;
    }
}

