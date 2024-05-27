using Base.Contracts.Domain;
using Base.Domain;
using Base.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Domain
{
    
    public class BagWithLetters : Bag
    {
        [Required]
        [DisplayName("Count of letters")]
        [Range(1, int.MaxValue, ErrorMessage = "Count of letters must be greater than 0")]
        public int CountOfLetters { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Weight must be a positive number.")]
        [DecimalPrecision(3, ErrorMessage = "Weight cannot have more than 3 decimal places.")]
        public decimal Weight { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        [DecimalPrecision(2, ErrorMessage = "Price cannot have more than 2 decimal places.")]
        public decimal Price { get; set; }
    }
}
