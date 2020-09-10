using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class ImportUsersDTO
    {
        [Required]
        [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$")]
        [JsonProperty("FullName")]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        [JsonProperty("Email")]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        [JsonProperty("Username")]
        public string Username { get; set; }
        [Required]
        [Range(3,103)]
        [JsonProperty("Age")]
        public int Age { get; set; }
        [Required]
        public ImportUsersCardsDTO[] Cards { get; set; }
    }
}
