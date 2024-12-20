using System;
using Attrition.PlayerCharacter;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEditor.SceneManagement;
using Object = System.Object;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find with Script", story: "Find [Target] with Script: [ScriptName]", category: "Action/Find", id: "5e8068a61e4784a51b4f19e4f30b0c58")]
public partial class FindWithScriptAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<string> ScriptName;

    protected override Status OnStart()
    {
        Target.Value = GameObject.Find(ScriptName.Value);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

