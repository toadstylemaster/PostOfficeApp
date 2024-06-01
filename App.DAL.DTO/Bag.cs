using Base.Contracts.Domain;
using Base.Domain;
using System.ComponentModel.DataAnnotations;

namespace App.DAL.DTO
{
    public class Bag : DomainEntityId<Guid>, IDomainEntityId, IBag
    {
        [Required]
        [StringLength(15, ErrorMessage = "Bag number cannot exceed 15 characters.")]
        [RegularExpression(@"^[A-Za-z0-9]*$", ErrorMessage = "Bag number can only contain alphanumeric characters.")]
        public string BagNumber { get; set; } = default!;

        public Guid? ShipmentId { get; set; }

        public Shipment? Shipment { get; set; }
    }
}
