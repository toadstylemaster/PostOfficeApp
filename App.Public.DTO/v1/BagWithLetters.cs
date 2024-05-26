using Base.Contracts.Domain;
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
    public class BagWithLetters : Bag
    {
        public int CountOfLetters { get; set; }

        public decimal Weight { get; set; }

        public decimal Price { get; set; }
    }
}
