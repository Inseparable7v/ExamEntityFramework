using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("Purchases")]
    public class ExportUser
    {
        [XmlAttribute("username")]
        [Required]
        public string Username { get; set; }
        [XmlArray("Purchase")]
        public ExportPurchase[] Purchase { get; set; }
    }
}
