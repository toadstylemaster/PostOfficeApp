namespace App.BLL.DTO
{
    public class BagWithParcels : Bag
    {
        public ICollection<Parcel>? ListOfParcels { get; set; }

    }
}
