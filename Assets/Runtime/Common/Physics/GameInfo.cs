using UnityEngine;

namespace Attrition.Common.Physics
{
    public static class GameInfo 
    {
        public readonly struct Layer
        {
            public Layer(string name)
            {
                Name = name;
                LayerIndex = LayerMask.NameToLayer(name);
                Mask = LayerMask.GetMask(name);
            }

            public readonly string Name;
            public readonly int LayerIndex;
            public readonly int Mask;
        }

        public static readonly Layer
            Default = new("Default"),
            Ground = new("Ground"),
            Player = new("Player"),
            Targetable = new("Targetable");
    }
}
