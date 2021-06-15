using Prism.Mvvm;

namespace MaxToolsUi.Models
{
    public class PropertyModel : BindableBase
    {
        public const string VariesCandidate = "[Varies]";
        public string Name { get; }

        public string OriginalValue { get; }

        private string _value;
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public PropertyModel(string name, string value)
            => (Name, Value, OriginalValue) = (name, value, value);

        public bool IsMatch(string name, string value)
            => Name == name && (value == VariesCandidate || Value == value);
    }
}
