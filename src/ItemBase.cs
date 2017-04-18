namespace Rainbow.CodeGenerator
{
    using System;
    using Model;

    /// <summary>
    /// Represents a deserialized item.
    /// Exposes the SyncItem, which can be used to get the contents of the items.
    /// </summary>
    public abstract class ItemBase
    {
        /// <summary>
        /// Gets the Original Serialized Item
        /// </summary>
        internal virtual IItemData Item { get; }

        protected ItemBase(IItemData item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.Item = item;
        }
    }
}