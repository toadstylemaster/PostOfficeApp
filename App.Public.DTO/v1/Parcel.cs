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
    public class Parcel : DomainEntityId
    {
        public string ParcelNumber { get; set; } = default!;

        public string RecipientName { get; set; } = default!;

        public string DestinationCountry { get; set; } = default!;

        public decimal Weight { get; set; }

        public decimal Price { get; set; }
    }
}
