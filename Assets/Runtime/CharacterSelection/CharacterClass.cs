﻿using UnityEngine;

namespace Attrition.CharacterSelection
{
    [CreateAssetMenu(menuName = "Scriptables/Character Class")]
    public class CharacterClass : ScriptableObject
    {
        [SerializeField]
        private string displayName;

        public string DisplayName
        {
            get => this.displayName;
            private set => this.displayName = value;
        }
    }
}
