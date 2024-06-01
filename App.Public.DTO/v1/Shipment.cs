using Base.Domain;

namespace App.Public.DTO.v1
{
    public class Shipment : DomainEntityId
    {
        public string ShipmentNumber { get; set; } = default!;

        public string Airport { get; set; } = default!;

        public string FlightNumber { get; set; } = default!;

        public DateTime FlightDate { get; set; }

        public ICollection<Bag>? ListOfBags { get; set; }

        public bool IsFinalized { get; set; }
    }
}
