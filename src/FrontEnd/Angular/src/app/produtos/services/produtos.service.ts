import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http"
import { Produto } from "../models/produto"
import { Categoria } from "../models/categoria"

import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { BaseService } from 'src/app/services/base.service';

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

    obterCategorias() : Observable<Categoria[]>{

        let url = this.UrlServiceV1 + 'api/vitrine/categorias';

        return this.http.get<any>(url).pipe(
            map(response => response.data)
        );
    }

}