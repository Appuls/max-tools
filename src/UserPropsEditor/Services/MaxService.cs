using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserPropsEditor.Services
{
    public enum OnInitializedBehavior
    {
        None,
        ShowDialog,
    }

    public class MaxService
    {
        public event EventHandler OnSelectionChanged;
        public readonly OnInitializedBehavior OnInitializedBehavior;
        public readonly bool IsStub;

        public MaxService(OnInitializedBehavior onInitializedBehavior, bool isStub)
        {
            OnInitializedBehavior = onInitializedBehavior;
            IsStub = isStub;
        }
    }
}
