using Base.Contracts.Domain;
using Base.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Public.DTO.v1
{
    public class BagWithParcels : DomainEntityId, IBag
    {
        public string BagNumber { get; set; } = default!;

        public ICollection<Parcel>? ListOfParcels { get; set; }
    }
}
