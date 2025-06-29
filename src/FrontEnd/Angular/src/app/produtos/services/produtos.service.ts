import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http"
import { Produto } from "../models/produto"

import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { BaseService } from 'src/app/services/base.service';

@Injectable()
export class ProdutoService extends BaseService {
    constructor(private http: HttpClient) { super(); }

    obterProdutos() : Observable<Produto[]>{
        return this.http.get<Produto[]>(this.UrlServiceV1 + "api/produtos/lista")
    }
}