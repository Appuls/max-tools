using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MaxToolsUi.Models
{
    public class NodeModel
    {
        public string Name { get; }
        public ObservableCollection<PropertyModel> Properties { get; } = new ObservableCollection<PropertyModel>();

        public NodeModel(string name, IReadOnlyList<PropertyModel> properties)
        {
            Name = name;
            Properties.AddRange(properties);
        }

        public bool HasProperty(string name, string value)
            => Properties.Any(p => p.IsMatch(name, value));

        public PropertyModel GetProperty(string name)
            => Properties.FirstOrDefault(p => p.Name == name);

        public void AddProperty(string name, string value)
        {
            var property = GetProperty(name);
            if (property == null)
            {
                Properties.Add(new PropertyModel(name, value));
            }
            else
            {
                property.Value = value;
            }
        }

        public void RemoveProperty(string name)
        {
            var toRemove = GetProperty(name);
            if (toRemove == null)
                return;
            Properties.Remove(toRemove);
        }
    }
}
