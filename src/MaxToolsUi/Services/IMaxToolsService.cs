using System;
using System.Collections.Generic;
using System.Windows;
using MaxToolsUi.Models;

namespace MaxToolsUi.Services
{
    public interface IMaxToolsService
    {
        /// <summary>
        /// Returns true if the backing service is a stub (for debugging purposes)
        /// </summary>
        bool IsStub { get; }

        /// <summary>
        /// An event handler invoked when the selection has changed.
        /// </summary>
        event EventHandler<SelectionChangedEventArgs> OnSelectionChanged;

        /// <summary>
        /// Controls the initialized behavior of the application.
        /// </summary>
        OnInitializedBehavior OnInitializedBehavior { get; }
        
        /// <summary>
        /// Controls the closing behavior of the window.
        /// </summary>
        OnClosingBehavior OnClosingBehavior { get; }

        /// <summary>
        /// Attaches the given window to the owner application.
        /// </summary>
        void AttachOwnerWindow(Window window);

        /// <summary>
        /// Enables or disables the selection observer.
        /// </summary>
        void ObserveSelectionChanged(bool enabled);

        /// <summary>
        /// Updates the selection based on the given property info.
        /// </summary>
        void SelectByProperty(string name, string value, bool add);

        /// <summary>
        /// Updates the selection with items that have no properties.
        /// </summary>
        void SelectByAbsentProperties(bool add);

        /// <summary>
        /// Refresh the current selection; invoked when the tool is activated.
        /// </summary>
        void RefreshSelection();

        /// <summary>
        /// Adds the given property to the set of node models.
        /// </summary>
        void AddProperty(string name, string value);
        
        /// <summary>
        /// Intended as a means to apply the state of the service's node models to the actual inodes in the selection.
        /// </summary>
        void ApplyNodeModels();
    }

    public enum OnInitializedBehavior
    {
        None,
        ShowDialog,
    }

    public enum OnClosingBehavior
    {
        None,
        Hide,
    }

    public class SelectionChangedEventArgs
    {
        public readonly IReadOnlyList<NodeModel> NodeModels;

        public SelectionChangedEventArgs(IReadOnlyList<NodeModel> nodeModels)
            => NodeModels = nodeModels ?? new List<NodeModel>();
    }
}
