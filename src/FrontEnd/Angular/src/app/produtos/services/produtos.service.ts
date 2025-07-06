import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http"
import { Produto } from "../models/produto"
import { Categoria } from "../models/categoria"

import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { BaseService } from 'src/app/services/base.service';
import { Favorito } from "../models/favorito";

@Injectable()
export class ProdutoService extends BaseService {
    constructor(private http: HttpClient) { super(); }

    public urlImagem = this.UrlImagemV1;
    obterProdutos(categoriaId?: string) : Observable<Produto[]>{

        let url = this.UrlServiceV1 + 'api/vitrine';
        if (categoriaId) {
            url += `?categoriaId=${categoriaId}`;
        }

        return this.http.get<any>(url).pipe(
            map(response => response.data)
        );
    }

    obterProdutosPorVendedor(vendedorId?: string) : Observable<Produto[]>{

        let url = this.UrlServiceV1 + `api/vitrine/por-vendedor/${vendedorId}`;

        return this.http.get<any>(url)
                        .pipe(map(response => response.data));
    }

    obterProduto(produtoId?: string) : Observable<Produto>{

        let url = this.UrlServiceV1 + 'api/vitrine/detalhe/';
        if (produtoId) {
            url += produtoId;
        }

        return this.http.get<any>(url).pipe(
            map(response => response.data)
        );
    }

    obterCategorias() : Observable<Categoria[]>{

        let url = this.UrlServiceV1 + 'api/vitrine/categorias';

        return this.http.get<any>(url).pipe(
            map(response => response.data)
        );
    }

    obterFavoritos(): Observable<Favorito[]> {
        let url = this.UrlServiceV1 + 'api/cliente/favoritos';

        return this.http.get<any>(url, this.ObterAuthHeaderJson()).pipe(
            map(response => response.data as Favorito[]),
            catchError(super.serviceError)
        );
    }

    adicionarFavorito(produtoId: string): Observable<void> {

        let url = this.UrlServiceV1 + `api/cliente/favoritos/${produtoId}`;

        return this.http.post<void>(url, {}, this.ObterAuthHeaderJson()).pipe(
            catchError(super.serviceError)
        );
    }

    removerFavorito(produtoId: string): Observable<void> {

        let url = this.UrlServiceV1 + `api/cliente/favoritos/${produtoId}`;

        return this.http.delete<void>(url, this.ObterAuthHeaderJson()).pipe(
            catchError(super.serviceError)
        );
    }
}