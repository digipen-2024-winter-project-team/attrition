using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Attrition.Common.Physics
{
    public class CollisionAggregate : MonoBehaviour
    {
        public bool Touching => touching;

        private bool touching;
        private List<Collider> colliders;

        public List<Collider> Colliders
        {
            get
            {
                colliders.RemoveAll(collider => collider == null);
                return colliders;
            }
        }

        private void Awake()
        {
            colliders = new();
        }
        private void OnTriggerStay(Collider other)
        {
            if (!colliders.Contains(other))
            {
                colliders.Add(other);
            }
        }

        private void FixedUpdate()
        {
            touching = colliders.Count > 0;

            colliders.Clear();
        }
    }
}
