using Base.Contracts.Domain;
using Base.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.DTO
{
    public class BagWithParcels : Bag
    {
        public ICollection<Parcel>? ListOfParcels { get; set; }

        public Guid? ShipmentId { get; set; }
        public Shipment? Shipment { get; set; }
    }
}
