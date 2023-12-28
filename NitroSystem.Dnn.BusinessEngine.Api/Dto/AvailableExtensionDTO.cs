using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Api.Dto
{
    public class AvailableExtensionDTO
    {
        [JsonProperty(PropertyName = "extensionName")]
        public string ExtensionName { get; set; }

        [JsonProperty(PropertyName = "extensionFile")]
        public string ExtensionFile { get; set; }

        [JsonProperty(PropertyName = "manifestFile")]
        public string ManifestFile { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "isFree")]
        public bool IsFree { get; set; }

        [JsonProperty(PropertyName = "isTrial")]
        public bool IsTrial { get; set; }

        [JsonProperty(PropertyName = "trialDays")]
        public int TrialDays { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "demo")]
        public string Demo { get; set; }

        [JsonProperty(PropertyName = "ownerCompany")]
        public string OwnerCompany { get; set; }

        [JsonProperty(PropertyName = "ownerWebsite")]
        public string OwnerWebsite { get; set; }

        [JsonProperty(PropertyName = "ownerEmail")]
        public string OwnerEmail { get; set; }
    }
}
