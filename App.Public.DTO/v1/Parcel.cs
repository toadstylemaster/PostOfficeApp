using Base.Domain;

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
