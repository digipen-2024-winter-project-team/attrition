using UnityEngine;

namespace Attrition.DamageSystem
{
    /// <summary>
    /// Struct for carrying damage result information from receiver back to dealer.
    /// </summary>
    public readonly struct DamageResult
    {
        /// <summary>
        /// Actual damage received by receiver.
        /// </summary>
        public readonly float value;
        /// <summary>
        /// Whether the damage was completely ignored or not.
        /// </summary>
        public readonly bool ignored;
        /// <summary>
        /// The GameObject which received the damage.
        /// </summary>
        public readonly GameObject receiver;
        /// <summary>
        /// The Combat Team of the receiver.
        /// </summary>
        public readonly CombatTeam receiverTeam;
        
        /// <summary>
        /// Constructs a new Damage Result object. Handled Internally
        /// </summary>
        public DamageResult(float value, bool ignored, GameObject receiver, CombatTeam receiverTeam)
        {
            this.value = value;
            this.ignored = ignored;
            this.receiver = receiver;
            this.receiverTeam = receiverTeam;
        }

        public override string ToString()
            => $"Receiver: {receiver.name}   Value: {value}   Ignored: {ignored}   Receiver Team: {receiverTeam.name}";
    }
}
