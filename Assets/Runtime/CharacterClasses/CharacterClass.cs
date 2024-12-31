using UnityEngine;
using UnityEngine.Serialization;

namespace Attrition.CharacterClasses
{
    [CreateAssetMenu(menuName = "Attrition/Characters/Character Class")]
    public class CharacterClass : ScriptableObject
    {
        [SerializeField]
        private string displayName;

        [SerializeField] private float hitpoints;
        [SerializeField] private float damageMitigation;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackSpeed;
        [SerializeField] private float damageZoneLength;
        [SerializeField] private float damageZoneWidth;
        [SerializeField] private float dodgeDistance;
        [SerializeField] private float dodgeRecharge;

        public string DisplayName
        {
            get => this.displayName;
            private set => this.displayName = value;
        }
        
        public float Hitpoints 
        {
            get => hitpoints;
            private set => hitpoints = value;
        }
        
        public float DamageMitigation 
        {
            get => damageMitigation;
            private set => damageMitigation = value;
        }
        
        public float WalkSpeed 
        {
            get => walkSpeed;
            private set => walkSpeed = value;
        }
        
        public float AttackDamage 
        {
            get => attackDamage;
            private set => attackDamage = value;
        }
        
        public float AttackSpeed 
        {
            get => attackSpeed;
            private set => attackSpeed = value;
        }
        
        public float DamageZoneLength 
        {
            get => damageZoneLength;
            private set => damageZoneLength = value;
        }
        
        public float DamageZoneWidth 
        {
            get => damageZoneWidth;
            private set => damageZoneWidth = value;
        }
        
        public float DodgeDistance 
        {
            get => dodgeDistance;
            private set => dodgeDistance = value;
        }
        
        public float DodgeRecharge 
        {
            get => dodgeRecharge;
            private set => dodgeRecharge = value;
        }
    }
}
