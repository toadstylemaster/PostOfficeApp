using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain;
using Base.Domain;

namespace App.Public.DTO.v1
{
    public class Shipment : DomainEntityId
    {
        public string ShipmentNumber { get; set; } = default!;

        public Airport Airport { get; set; }

        public string FlightNumber { get; set; } = default!;

        public DateTime FlightDate { get; set; }

        public ICollection<string> ListOfBags { get; set; } = default!;
    }
}
