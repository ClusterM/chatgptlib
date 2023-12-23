using System.Collections;
using System.Text.Json.Serialization;

namespace wtf.cluster.ChatGptLib.Types.Content
{
    /// <summary>
    /// Class that represents multipart content
    /// </summary>
    public class ChatContentParts : IChatContent, IList<IChatContentPart>
    {
        /// <summary>
        /// Content parts.
        /// </summary>
        private IList<IChatContentPart> parts;

        /// <summary>
        /// Get the number of elements (parts).
        /// </summary>
        public int Count => parts.Count;

        /// <summary>
        /// Check if the collection is read only.
        /// </summary>
        public bool IsReadOnly => parts.IsReadOnly;

        /// <summary>
        /// Access by index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>IChatContentPart object.</returns>
        public IChatContentPart this[int index] {
            get => parts[index];
            set => parts[index] = value; 
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChatContentParts()
        {
            parts = new List<IChatContentPart>();
        }

        /// <summary>
        /// ChatContentParts constructor.
        /// </summary>
        /// <param name="parts">Content parts.</param>
        public ChatContentParts(IList<IChatContentPart> parts)
        {
            this.parts = parts;
        }

        /// <summary>
        /// ChatContentParts constructor.
        /// </summary>
        /// <param name="parts">Content parts.</param>
        public ChatContentParts(params IChatContentPart[] parts)
        {
            this.parts = parts;
        }

        /// <summary>
        /// Determines the index of a specific item in the collection.
        /// </summary>
        /// <param name="item">Item to search.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(IChatContentPart item) => parts.IndexOf(item);

        /// <summary>
        /// Inserts an item to the collection at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="item">Item to insert.</param>
        public void Insert(int index, IChatContentPart item) => parts.Insert(index, item);

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        public void RemoveAt(int index) => parts.RemoveAt(index);

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(IChatContentPart item) => parts.Add(item);

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear() => parts.Clear();

        /// <summary>
        /// Determines whether the collection contains a specific value.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>true if item is found in the collection; otherwise, false.</returns>
        public bool Contains(IChatContentPart item) => parts.Contains(item);

        /// <summary>
        /// Copies the elements of the collection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">Array.</param>
        /// <param name="arrayIndex">Index.</param>
        public void CopyTo(IChatContentPart[] array, int arrayIndex) => parts.CopyTo(array, arrayIndex);

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>true if item was successfully removed; otherwise, false. This method also returns false if item is not found.</returns>
        public bool Remove(IChatContentPart item) => parts.Remove(item);

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that iterates through the collection.</returns>
        public IEnumerator<IChatContentPart> GetEnumerator() => parts.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => parts.GetEnumerator();
    }
}
