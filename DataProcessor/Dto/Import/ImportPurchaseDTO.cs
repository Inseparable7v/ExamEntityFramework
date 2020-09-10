using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Import
{
    [XmlType("Purchase")]
    public class ImportPurchaseDTO
    {
        [XmlElement("Type")]
        [Required]
        public string Type { get; set; }
        [XmlElement("Key")]
        [RegularExpression("^([A-Z0-9]{4})-([A-Z0-9]{4})-([A-Z0-9]{4})$")]
        [Required]
        public string Key { get; set; }
        [XmlElement("Card")]
        [RegularExpression("^([0-9]{4}) ([0-9]{4}) ([0-9]{4}) ([0-9]{4})$")]
        [Required]
        public string Card { get; set; }
        [XmlElement("Date")]
        [Required]
        public string Date { get; set; }
        [XmlAttribute("title")]
        [Required]
        public string title { get; set; }
    }
}
