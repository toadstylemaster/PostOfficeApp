using Base.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain;
using Base.Helpers;

namespace App.BLL.DTO
{
    public class Shipment : DomainEntityId
    {
        [DisplayName("Shipment number")]
        [RegularExpression(@"[A-Za-z0-9]+-[A-Za-z0-9]+", ErrorMessage = "Shipment number must be in format: 'XXX-XXXXXX', where X-letter or digit")]
        public string ShipmentNumber { get; set; } = default!;

        public Airport Airport { get; set; }

        [DisplayName("Flight number")]
        [RegularExpression(@"[A-Za-z][A-Za-z]\d\d\d\d", ErrorMessage = "Flight number must be in format: 'LLNNNN', where L-letter, N-number")]
        public string FlightNumber { get; set; } = default!;

        [DisplayName("Flight date")]
        [DateGreaterThanOrEqualToToday]
        public DateTime FlightDate { get; set; }

        public ICollection<string>? ListOfBags { get; set; }
    }
}
