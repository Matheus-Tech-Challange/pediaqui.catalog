using AutoMapper;
using Domain.Produto.Entities;
using Domain.Produto.Extensions;

namespace Application.Features.ProdutoContext;

public class ProdutoMapper : Profile
{
    public ProdutoMapper()
    {
        CreateMap<Produto, ProdutoResponse>()
            .ForMember(dest => dest.Categoria, src => src.MapFrom(src => src.Categoria.ToText()));
    }
}
