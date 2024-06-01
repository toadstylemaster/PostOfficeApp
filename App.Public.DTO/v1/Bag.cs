using Base.Contracts.Domain;
using Base.Domain;

namespace App.Public.DTO.v1
{
    public class Bag : DomainEntityId<Guid>, IDomainEntityId, IBag
    {
        public string BagNumber { get; set; } = default!;

    }
}
