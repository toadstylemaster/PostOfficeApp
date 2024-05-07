using App.BLL.Contracts.Services;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using Base.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Services
{
    public class ParcelService : BaseEntityService<Parcel, DAL.DTO.Parcel, IParcelRepository>, IParcelService
    {
        public ParcelService(IParcelRepository repository, ParcelMapper mapper) : base(repository, mapper)
        {
        }

    }
}
