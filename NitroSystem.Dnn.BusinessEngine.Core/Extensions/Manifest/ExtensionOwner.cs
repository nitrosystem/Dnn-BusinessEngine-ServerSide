using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Extensions.Manifest
{
    public class ExtensionOwner
    {
        public Guid OwnerID { get; set; }
        public string OwnerName { get; set; }
        public string Organization { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
    }
}