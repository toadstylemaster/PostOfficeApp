using Base.Domain;

namespace App.Domain
{
    public class BagWithParcels: Bag
    {
        public ICollection<Parcel>? ListOfParcels { get; set; }
    }
}
