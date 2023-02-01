using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using ProductGrpc.Protos;

namespace ProductGrpc.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Models.Product, ProductModel>()
                .ForMember(destination => destination.CreatedTime,
                opt => opt.MapFrom(source => Timestamp.FromDateTime(source.CreatedTime)));

            CreateMap<ProductModel, Models.Product>()
                .ForMember(destination => destination.CreatedTime,
                opt => opt.MapFrom(source => source.CreatedTime.ToDateTime()));
        }
    }
}
