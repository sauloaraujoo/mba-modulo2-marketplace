﻿using AutoMapper;
using LojaVirtual.Business.Entities;
using LojaVirtual.Mvc.Models;

namespace LojaVirtual.Mvc.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Vendedor, VendedorViewModel>().ReverseMap();
            CreateMap<Categoria, CategoriaViewModel>().ReverseMap();
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.NomeCategoria, opt => opt.MapFrom(src => src.Categoria.Nome))
                .ForMember(dest => dest.NomeVendedor, opt => opt.MapFrom(src => src.Vendedor.Nome));
            CreateMap<ProdutoViewModel, Produto>();
            CreateMap<Produto, ProdutoVitrineViewModel>()
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria.Nome))
                .ForMember(dest => dest.Vendedor, opt => opt.MapFrom(src => src.Vendedor.Nome));
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
