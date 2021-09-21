using System.Collections.Generic;

namespace Elsa.Models
{
    /// <summary>
    /// Represents a register of memory. 
    /// </summary>
    public class Register
    {
        private readonly IDictionary<string, RegisterLocation> _locations;

        public Register(Register? parentRegister, IDictionary<string, RegisterLocation>? locations = default)
        {
            ParentRegister = parentRegister;
            _locations = locations ?? new Dictionary<string, RegisterLocation>();
        }

        public Register? ParentRegister { get; set; }
        public IReadOnlyDictionary<string, RegisterLocation> Locations => (IReadOnlyDictionary<string, RegisterLocation>)_locations;

        public bool TryGetLocation(string id, out RegisterLocation location)
        {
            var current = this;

            while (current != null)
            {
                if (current.Locations.TryGetValue(id, out location!))
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

        public RegisterLocation Declare(RegisterLocationReference reference)
        {
            var location = reference.Declare();
            _locations[reference.Id] = location;
            return location;
        }
    }
}