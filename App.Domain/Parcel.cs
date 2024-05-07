using Base.Domain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.Domain
{
    public class Parcel: DomainEntityId
    {
        [DisplayName("Parcel number")]
        [RegularExpression(@"[A-Za-z][A-Za-z]\d\d\d\d\d\d[A-Za-z][A-Za-z]", ErrorMessage = "Parcel numbers must follow pattern: 'LLNNNNNNLL', where L-letter, N-digit")]
        public string ParcelNumber { get; set; } = default!;

        [MaxLength(100, ErrorMessage = "Name is too long")]
        public string RecipientName { get; set; } = default!;

        [MinLength(2, ErrorMessage = "Country code is too short")]
        [MaxLength(2, ErrorMessage = "Country code is too long")]
        [RegularExpression(@"[A-Z]",
         ErrorMessage = "Only uppercase Characters are allowed.")]
        public string DestinationCountry { get; set;} = default!;

        [RegularExpression(@"^\d+.\d{0,3}$", ErrorMessage = "Weight can't have more than 3 decimal places")]
        public decimal Weight { get; set; }

        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Price can't have more than 2 decimal places")]
        public decimal Price { get; set; }
    }
}
