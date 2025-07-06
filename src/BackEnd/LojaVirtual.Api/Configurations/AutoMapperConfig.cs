using AutoMapper;
using LojaVirtual.Api.Models;
using LojaVirtual.Business.Entities;

namespace LojaVirtual.Api.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {            
            CreateMap<Categoria, CategoriaModel>().ReverseMap();
            CreateMap<Produto, ProdutoModel>().ForMember(dest => dest.NomeCategoria, opt => opt.MapFrom(src => src.Categoria.Nome))
                                              .ForMember(dest => dest.NomeVendedor, opt => opt.MapFrom(src => src.Vendedor.Nome));
            CreateMap<ProdutoModel, Produto>();

            //CreateMap<ProdutoImagemViewModel, Produto>().ReverseMap();
        }
    }
    public static class AutoMapperAdd
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
