using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Attrition.DamageSystem
{
    /// <summary>
    /// Class for carrying damage information from dealer to receiver, and reporting results back to dealer.
    /// </summary>
    public class DamageInfo
    {
        /// <summary>
        /// The raw damage value. Prefer to read from GetValue so that combat team can be factored in.
        /// </summary>
        public readonly float rawValue;
        /// <summary>
        /// The GameObject that dealt the damage.
        /// </summary>
        public readonly GameObject dealer;
        /// <summary>
        /// The CombatTeam of the damage dealer.
        /// </summary>
        public readonly CombatTeam team;

        private readonly List<DamageResult> results;
        
        /// <summary>
        /// Constructs a new DamageInfo object.
        /// </summary>
        /// <param name="rawValue"> The raw damage value. </param>
        /// <param name="dealer"> The GameObject dealing the damage. </param>
        /// <param name="team"> The CombatTeam of the damage dealer. </param>
        public DamageInfo(float rawValue, GameObject dealer, CombatTeam team)
        {
            this.rawValue = rawValue;
            this.dealer = dealer;
            this.team = team;
            this.results = new();
        }

        /// <summary>
        /// Get the calculated damage value based on the receiver's combat team and report damage results back to damage dealer.
        /// </summary>
        /// <param name="invincible"> Whether the damage was completely ignored or not. </param>
        /// <param name="team"> The combat team of the receiver. </param>
        /// <param name="receiver"> The GameObject receiving the damage. </param>
        /// <returns> The calculated damage value based on the receiver's combat team. </returns>
        public float Receive(bool invincible, CombatTeam team, GameObject receiver)
        {
            float value = GetValue(team);
            
            results.Add(new(value, invincible, receiver, team));

            return value;
        }

        /// <summary>
        /// Get a list of the damage results.
        /// </summary>
        public DamageResult[] GetResults() => results.ToArray();
        
        /// <summary>
        /// Get the calculated damage value based on the receiver's combat team.
        /// </summary>
        public float GetValue(CombatTeam receiverTeam)
        {
            if (receiverTeam == null || team == null) return rawValue;

            var interaction = team.GetInteraction(receiverTeam);
            if (interaction == null) return rawValue;

            return rawValue * interaction.damageMultiplier;
        }

        public override string ToString()
        {

            string summary = results.Any(result => !result.ignored) ? "Taken" : "Ignored";
            string allResults = string.Join("\n", results.Select(result => $" - {result}"));

            return $"(DamageInfo) Dealer: {dealer.name}   Raw Value: {rawValue}   Team: {team.name}   Summary: {summary}\nResults\n{allResults}";
        }
    }
}
