using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Game")]
    public class ExportGame
    {
        [XmlElement("Genre")]
        [Required]
        public string Genre { get; set; }
        [XmlElement("Price")]
        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }
        [XmlAttribute("title")]
        [Required]
        public string title { get; set; }
    }
}
