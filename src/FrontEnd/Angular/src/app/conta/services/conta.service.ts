import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Usuario } from '../models/usuario';

import { Observable } from 'rxjs';
import { catchError, map } from "rxjs/operators";
import { BaseService } from 'src/app/services/base.service';

import { Router } from '@angular/router';

@Injectable()
export class ContaService extends BaseService {

    constructor(private http: HttpClient, router: Router) { super(router); }

    registrarUsuario(usuario: Usuario): Observable<Usuario> {
        let response = this.http
            .post(this.UrlServiceV1 + 'auth/registrar', usuario, this.ObterHeaderJson())
            .pipe(
                map(this.extrairDados),
                catchError(this.serviceError));

        return response;
    }

    login(usuario: Usuario): Observable<Usuario> {
        let response = this.http
            .post(this.UrlServiceV1 + 'auth/login', usuario, this.ObterHeaderJson())
            .pipe(
                map(this.extrairDados),
                catchError(this.serviceError));

        return response;
    }
}