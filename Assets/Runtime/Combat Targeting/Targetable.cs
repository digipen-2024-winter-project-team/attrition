using System;
using System.Collections.Generic;
using Attrition.DamageSystem;
using UnityEngine;

namespace Attrition.CombatTargeting
{
    public class Targetable : MonoBehaviour
    {
        [SerializeField] private CombatTeam team;

        public CombatTeam Team => team;

        private static List<Targetable> targetables = new();
        public static List<Targetable> Targetables => targetables;
        
        private void OnEnable()
        {
            targetables.Add(this);
        }

        private void OnDisable()
        {
            targetables.Remove(this);
        }
    }
}
