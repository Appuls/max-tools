using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MaxToolsUi.Models
{
    public class PropertyEntry
    {
        public string Guid { get; }
        public bool IsGlobal { get; }
        public string Name { get; }
        public ObservableCollection<string> CandidateValues { get; } = new ObservableCollection<string>();

        public PropertyEntry(string name, IReadOnlyList<string> candidateValues, bool isGlobal)
        {
            Guid = System.Guid.NewGuid().ToString();
            Name = name;
            IsGlobal = isGlobal;

            if (candidateValues.Count > 1)
            {
                CandidateValues.Add(PropertyModel.VariesCandidate);
            }
            CandidateValues.AddRange(candidateValues);
        }
    }
}
