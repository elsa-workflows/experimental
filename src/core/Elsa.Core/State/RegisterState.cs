using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.State
{
    public class RegisterState
    {
        public RegisterState(IDictionary<string, RegisterLocation> locations, RegisterState? parentRegister = default)
        {
            Locations = locations;
            ParentRegister = parentRegister;
        }

        public IDictionary<string, RegisterLocation> Locations { get; set; }
        public RegisterState? ParentRegister { get; set; }
    }
}