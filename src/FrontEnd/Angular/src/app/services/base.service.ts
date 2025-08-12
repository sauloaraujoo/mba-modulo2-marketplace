import { HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { throwError } from "rxjs";
import { environment } from "src/environments/environment";
import { LocalStorageUtils } from '../utils/localstorage';
import { Router } from '@angular/router';

export abstract class BaseService {

    constructor(private router: Router) {}

    protected UrlServiceV1: string = environment.apiUrlv1;
    protected UrlImagemV1: string = environment.imagemUrlv1;
    public LocalStorage = new LocalStorageUtils();


    protected ObterHeaderJson() {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
    }

    protected ObterAuthHeaderJson() {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.LocalStorage.obterTokenUsuario()}`
            })
        };
    }

    protected extrairDados(response: any) {
        return response.data || {};
    }

    protected serviceError(response: Response | any) {
        let customError: string[] = [];
        let customResponse: { error: { errors: string[] } } = { error: { errors: [] } }

        if (response instanceof HttpErrorResponse) {

            if (response.statusText === "Unknown Error") {
                customError.push("Ocorreu um erro desconhecido");
                response.error.errors = customError;
            }
        }

        if (response.status === 500) {
            customError.push("Ocorreu um erro no processamento, tente novamente mais tarde ou contate o nosso suporte.");
            customResponse.error.errors = customError;
            return throwError(customResponse);
        }
        else if (response.status === 404) {
            this.router.navigate(['/nao-encontrado']);
        }
        return throwError(response);
    }

    public usuarioLogado(): boolean {
        return !!this.LocalStorage.obterTokenUsuario();
    }
}