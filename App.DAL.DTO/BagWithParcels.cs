using Base.Contracts.Domain;
using Base.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.DTO
{
    public class BagWithParcels : DomainEntityId, IBag
    {
        [DisplayName("Bag number")]
        [MaxLength(15)]
        public string BagNumber { get; set; } = default!;

        public ICollection<Parcel>? ListOfParcels { get; set; }
    }
}
