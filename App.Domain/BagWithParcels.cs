using Base.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Contracts.Domain;

namespace App.Domain
{
    public class BagWithParcels: DomainEntityId, IBag
    {
        [DisplayName("Bag number")]
        [MaxLength(15)]
        public string BagNumber { get; set; } = default!;

        public ICollection<Parcel>? ListOfParcels { get; set; }
    }
}
