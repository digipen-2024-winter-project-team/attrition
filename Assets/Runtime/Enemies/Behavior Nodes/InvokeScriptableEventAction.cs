using Attrition.Common.Events.ScriptableEvents;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Invoke ScriptableEvent", story: "Invoke [ScriptableEvent]", category: "Action", id: "2ba33c56227447ffaac1d3b2645d2822")]
public partial class InvokeScriptableEventAction : Action
{
    [SerializeReference] public BlackboardVariable<ScriptableEvent> ScriptableEvent;

    protected override Status OnStart()
    {
        this.ScriptableEvent.Value.Invoke();
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}
