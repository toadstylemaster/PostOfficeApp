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
    public class BagWithLetters : DomainEntityId, IBag
    {

        [DisplayName("Bag number")]
        [MaxLength(15)]
        public string BagNumber { get; set; } = default!;

        [DisplayName("Count of letters")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Count of letters must be greater than 0")]
        public int CountOfLetters { get; set; }

        [RegularExpression(@"^\d+.\d{0,3}$", ErrorMessage = "Weight can't have more than 3 decimal places")]
        public decimal Weight { get; set; }

        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Price can't have more than 2 decimal places")]
        public decimal Price { get; set; }
    }
}
