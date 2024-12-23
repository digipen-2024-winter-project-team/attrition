using System.Collections.Generic;
using UnityEngine;

namespace Attrition.Names.Generation
{
    [CreateAssetMenu(fileName = "NameData", menuName = "Scriptables/Name Data")]
    public class NameData : ScriptableObject, INameData
    {
        [SerializeField]
        private List<string> prefixes;
        [SerializeField]
        private List<string> roots;
        [SerializeField]
        private List<string> suffixes;

        public List<string> Prefixes => this.prefixes;
        public List<string> Roots => this.roots;
        public List<string> Suffixes => this.suffixes;
    }
}
