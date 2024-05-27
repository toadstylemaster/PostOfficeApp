using Base.Contracts.Domain;
using Base.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Public.DTO.v1
{
    [NotMapped]
    public class Bag : DomainEntityId<Guid>, IDomainEntityId, IBag
    {
        public string BagNumber { get; set; } = default!;

        public Guid? ShipmentId { get; set; }

        public Shipment? Shipment { get; set; }
    }
}
