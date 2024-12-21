using System;
using System.Collections.Generic;
using UnityEngine;

namespace Attrition.DamageSystem
{
    [CreateAssetMenu(menuName = "Attrition/Combat/Combat Team")]
    public class CombatTeam : ScriptableObject
    {
        [SerializeField] private List<Interaction> interactions;

        public Interaction GetInteraction(CombatTeam team)
            => interactions.Find(interaction => interaction.team == team);
        
        [Serializable]
        public class Interaction
        {
            public CombatTeam team;
            public float damageMultiplier;
        }
    }
}
