using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace VaporStore.DataProcessor.Dto
{
    public class ImportGamesDTO
    {
        [Required]
        public string Name { get; set; }
        [JsonProperty("Price")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [Required]
        public decimal Price { get; set; }
        [JsonProperty("ReleaseDate")]
        [Required]
        public string ReleaseDate { get; set; }
        [JsonProperty("Developer")]
        [Required]
        public string Developer { get; set; }
        [JsonProperty("Genre")]
        [Required]
        public string Genre { get; set; }
        [Required]
        public string[] Tags { get; set; }
    }
}
