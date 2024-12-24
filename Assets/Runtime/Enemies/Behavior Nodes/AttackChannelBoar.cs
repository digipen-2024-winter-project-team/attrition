using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/Attack")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "Attack", message: "[Self] attacks [player]", category: "Events", id: "66ac6e2cfd4ecb49111a47b590b310f8")]
public partial class AttackChannelBoar : EventChannelBase
{
    public delegate void AttackChannelBoarEventHandler(GameObject Self, GameObject player);
    public event AttackChannelBoarEventHandler Event; 

    public void SendEventMessage(GameObject Self, GameObject player)
    {
        Event?.Invoke(Self, player);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> SelfBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Self = SelfBlackboardVariable != null ? SelfBlackboardVariable.Value : default(GameObject);

        BlackboardVariable<GameObject> playerBlackboardVariable = messageData[1] as BlackboardVariable<GameObject>;
        var player = playerBlackboardVariable != null ? playerBlackboardVariable.Value : default(GameObject);

        Event?.Invoke(Self, player);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        AttackChannelBoarEventHandler del = (Self, player) =>
        {
            BlackboardVariable<GameObject> var0 = vars[0] as BlackboardVariable<GameObject>;
            if(var0 != null)
                var0.Value = Self;

            BlackboardVariable<GameObject> var1 = vars[1] as BlackboardVariable<GameObject>;
            if(var1 != null)
                var1.Value = player;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as AttackChannelBoarEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as AttackChannelBoarEventHandler;
    }
}

