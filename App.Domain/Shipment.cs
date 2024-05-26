using Base.Domain;
using Base.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class Shipment: DomainEntityId
    {
        [Required]
        [DisplayName("Shipment number")]
        [RegularExpression(@"^[A-Za-z0-9]{3}-[A-Za-z0-9]{6}$", ErrorMessage = "Shipment number must be in format: 'XXX-XXXXXX', where X-letter or digit")]
        public string ShipmentNumber { get; set; } = default!;

        [Required]
        [RegularExpression(@"^(TLL|RIX|HEL)$", ErrorMessage = "Airport must be one of the following: 'TLL', 'RIX', 'HEL'.")]
        public Airport Airport { get; set; }

        [DisplayName("Flight number")]
        [RegularExpression(@"^[A-Za-z]{2}\d{4}$", ErrorMessage = "Flight number must be in format: 'LLNNNN', where L-letter, N-number")]
        public string FlightNumber { get; set; } = default!;

        [DisplayName("Flight date")]
        [DateGreaterThanOrEqualToToday]
        public DateTime FlightDate { get; set; }

        public ICollection<Bag>? ListOfBags { get; set; }

        public bool IsFinalized { get; set; }
    }
}
