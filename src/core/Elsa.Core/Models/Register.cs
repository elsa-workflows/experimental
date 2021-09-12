using System.Collections.Generic;

namespace Elsa.Models
{
    /// <summary>
    /// Represents a register of memory. 
    /// </summary>
    public class Register
    {
        private readonly IDictionary<string, RegisterLocation> _locations = new Dictionary<string, RegisterLocation>();

        public Register(Register? parentRegister)
        {
            ParentRegister = parentRegister;
        }
        
        public Register? ParentRegister { get; }
        public IReadOnlyDictionary<string, RegisterLocation> Locations => (IReadOnlyDictionary<string, RegisterLocation>)_locations;

        public bool TryGetLocation(string id, out RegisterLocation location)
        {
            var current = this;

            while (current != null)
            {
                if (Locations.TryGetValue(id, out location!))
                    return true;

                current = current.ParentRegister;
            }

            location = null!;
            return false;
        }

        public void Declare(IEnumerable<RegisterLocationReference> references)
        {
            foreach (var reference in references)
            {
                var location = reference.Declare();
                _locations[reference.Id] = location;
            }
        }
    }
}