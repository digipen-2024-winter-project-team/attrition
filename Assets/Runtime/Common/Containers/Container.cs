using System.Collections.Generic;
using UnityEngine;

namespace Attrition.Common.Containers
{
    /// <summary>
    /// A generic container that manages a list of items of type <typeparamref name="T"/>.
    /// Provides basic functionality for adding and removing items.
    /// </summary>
    /// <typeparam name="T">The type of items stored in the container.</typeparam>
    public abstract class Container<T> : MonoBehaviour
    {
        /// <summary>
        /// The internal list that holds the container's contents.
        /// </summary>
        [SerializeField]
        private List<T> contents;

        /// <summary>
        /// Provides read-only access to the items in the container.
        /// </summary>
        /// <value>
        /// An <see cref="IEnumerable{T}"/> representing the items currently stored in the container.
        /// </value>
        /// <remarks>
        /// The <c>Contents</c> property allows external code to query or iterate over the items
        /// in the container without exposing the ability to directly modify the underlying list.
        /// </remarks>
        public IEnumerable<T> Contents => this.contents;

        /// <summary>
        /// Provides indexed access to items in the container.
        /// </summary>
        /// <param name="index">The zero-based index of the item to access.</param>
        /// <returns>The item at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown if the index is out of range.
        /// </exception>
        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < this.contents.Count)
                {
                    return this.contents[index];
                }

                Debug.LogError($"Index {index} is out of range for the container.");
                throw new System.ArgumentOutOfRangeException(nameof(index), $"Index {index} is out of range.");
            }
        }
        
        /// <summary>
        /// Initializes the container. Creates a new instance of the contents list.
        /// </summary>
        private void Awake()
        {
            this.contents = new();
        }

        /// <summary>
        /// Adds an item to the container if it is not null and not already present.
        /// </summary>
        /// <param name="item">The item to add to the container.</param>
        /// <remarks>
        /// If the item is null or already exists in the container, a warning will be logged.
        /// </remarks>
        public virtual void Add(T item)
        {
            if (item == null)
            {
                Debug.LogWarning("Attempted to add a null item to the container.");
                return;
            }

            if (this.contents.Contains(item))
            {
                Debug.LogWarning($"Item {item} is already in the container.");
                return;
            }

            this.contents.Add(item);
        }

        /// <summary>
        /// Removes an item from the container if it exists.
        /// </summary>
        /// <param name="item">The item to remove from the container.</param>
        /// <remarks>
        /// If the item is null or not found in the container, a warning will be logged.
        /// </remarks>
        public virtual void Remove(T item)
        {
            if (item == null)
            {
                Debug.LogWarning("Attempted to remove a null item from the container.");
                return;
            }

            if (!this.contents.Contains(item))
            {
                Debug.LogWarning($"Item {item} was not found in the container.");
                return;
            }

            this.contents.Remove(item);
        }
    }
}
