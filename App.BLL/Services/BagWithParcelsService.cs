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
    public class BagWithParcelsService : BaseEntityService<BagWithParcels, DAL.DTO.BagWithParcels, IBagWithParcelsRepository>, IBagWithParcelsService
    {
        public BagWithParcelsService(IBagWithParcelsRepository repository, BagWithParcelsMapper mapper) : base(repository, mapper)
        {
        }

    }
}
