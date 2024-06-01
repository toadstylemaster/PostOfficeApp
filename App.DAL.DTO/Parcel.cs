using Base.Domain;
using Base.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.DAL.DTO
{
    public class Parcel : DomainEntityId
    {
        [Required]
        [DisplayName("Parcel number")]
        [RegularExpression(@"^[A-Za-z]{2}\d{6}[A-Za-z]{2}$", ErrorMessage = "Parcel numbers must follow pattern: 'LLNNNNNNLL', where L-letter, N-digit")]
        public string ParcelNumber { get; set; } = default!;

        [Required]
        [StringLength(100, ErrorMessage = "Name is too long")]
        public string RecipientName { get; set; } = default!;

        [Required]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Destination country must be a 2-letter code, only uppercase characters are allowed!")]
        public string DestinationCountry { get; set; } = default!;


        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Weight must be a positive number.")]
        [DecimalPrecision(3, ErrorMessage = "Weight cannot have more than 3 decimal places.")]
        public decimal Weight { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        [DecimalPrecision(2, ErrorMessage = "Price cannot have more than 2 decimal places.")]
        public decimal Price { get; set; }

        public BagWithParcels? BagWithParcels { get; set; }
        public Guid? BagWithParcelsId { get; set; }
    }
}
