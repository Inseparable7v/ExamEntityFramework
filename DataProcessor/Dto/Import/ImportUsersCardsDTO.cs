using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class ImportUsersCardsDTO
    {
        [JsonProperty("Number")]
        [RegularExpression("^([0-9]{4}) ([0-9]{4}) ([0-9]{4}) ([0-9]{4})$")]
        [Required]
        public string Number { get; set; }
        [JsonProperty("CVC")]
        [RegularExpression("^[0-9]{3}$")]
        [Required]
        public string CVC { get; set; }
        [JsonProperty("Type")]
        [Required]
        public string Type { get; set; }
    }
}
