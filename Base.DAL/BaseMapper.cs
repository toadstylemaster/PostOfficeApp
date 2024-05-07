using AutoMapper;
using Base.Contracts.Base;

namespace Base.DAL
{
    public class BaseMapper<TIn, TOut> : IMapper<TIn, TOut>
    {
        protected readonly IMapper Mapper;

        public BaseMapper(IMapper mapper)
        {
            Mapper = mapper;
        }

        public TOut? Map(TIn? entity)
        {
            return Mapper.Map<TOut>(entity);
        }

        public TIn? Map(TOut? entity)
        {
            return Mapper.Map<TIn>(entity);
        }
    }
}
