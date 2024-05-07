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
    public class BagWithLetters : DomainEntityId, IBag
    {
        public string BagNumber { get; set; } = default!;

        public int CountOfLetters { get; set; }

        public decimal Weight { get; set; }

        public decimal Price { get; set; }
    }
}
