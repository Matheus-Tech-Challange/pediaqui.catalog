﻿using Domain.Produto.Enuns;

namespace Domain.Produto.Extensions;

public static class CategoriaProdutoExtension
{
    public static int ToInt(this CategoriaProduto categoria)
    {
        return (int)categoria;
    }

    public static string ToText(this CategoriaProduto categoria)
    {
        switch (categoria)
        {
            case CategoriaProduto.LANCHE:
                return nameof(CategoriaProduto.LANCHE);
            case CategoriaProduto.ACOMPANHAMENTO:
                return nameof(CategoriaProduto.ACOMPANHAMENTO);
            case CategoriaProduto.BEBIDA:
                return nameof(CategoriaProduto.BEBIDA);
            case CategoriaProduto.SOBREMESA:
                return nameof(CategoriaProduto.SOBREMESA);
            default:
                throw new ArgumentException($"Categoria de produto '{categoria}' não existe!");
        }
    }

    public static CategoriaProduto ToCategoriaProduto(this int categoria)
    {
        switch (categoria)
        {
            case (int)CategoriaProduto.LANCHE:
                return CategoriaProduto.LANCHE;
            case (int)CategoriaProduto.ACOMPANHAMENTO:
                return CategoriaProduto.ACOMPANHAMENTO;
            case (int)CategoriaProduto.BEBIDA:
                return CategoriaProduto.BEBIDA;
            case (int)CategoriaProduto.SOBREMESA:
                return CategoriaProduto.SOBREMESA;
            default:
                throw new ArgumentException($"Categoria de produto '{categoria}' não existe!");
        }
    }
}
