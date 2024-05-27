
namespace App.Public.DTO.v1
{
    public class BagWithParcels : Bag
    {
        public ICollection<Parcel>? ListOfParcels { get; set; }
    }
}
