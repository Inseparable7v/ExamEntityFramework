using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Purchase")]
    public class ExportPurchase
    {
        [XmlElement("Card")]
        [Required]
        [RegularExpression(@"^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$")]
        public string CardNumber { get; set; }
        [XmlElement("Cvc")]
        [Required]
        [RegularExpression(@"^[0-9]{3}$")]
        public string Cvc { get; set; }
        [XmlElement("Date")]
        [Required]
        public string Date { get; set; }
        [XmlArray("Game")]
        public ExportGame[] Game { get; set; }
    }
}
