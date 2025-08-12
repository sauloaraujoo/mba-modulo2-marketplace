import { HttpHeaders, HttpErrorResponse } from "@angular/common/http";
import { throwError } from "rxjs";
import { environment } from "src/environments/environment";
import { LocalStorageUtils } from '../utils/localstorage';
import { Router } from '@angular/router';
import { inject } from "@angular/core";

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

        //const router = inject(Router);

        if (response instanceof HttpErrorResponse) {

            if (response.statusText === "Unknown Error") {
                customError.push("Ocorreu um erro desconhecido");
                response.error.errors = customError;
            }
        }
        console.log(response);
        if (response.status === 500) {
            customError.push("Ocorreu um erro no processamento, tente novamente mais tarde ou contate o nosso suporte.");
            
            // Erros do tipo 500 não possuem uma lista de erros
            // A lista de erros do HttpErrorResponse é readonly                
            customResponse.error.errors = customError;
            return throwError(customResponse);
        }
        else if (response.status === 404) {
            
            //var router = new Router();

            //window.location.href = '/nao-encontrado';
            this.router.navigate(['/nao-encontrado']);
            customError.push("Ocorreu um erro no processamento, tente novamente mais tarde ou contate o nosso suporte.");
            
            // Erros do tipo 500 não possuem uma lista de erros
            // A lista de erros do HttpErrorResponse é readonly                
            customResponse.error.errors = customError;
            return throwError(customResponse);
        }


        console.error(response);
        return throwError(response);
    }

    public usuarioLogado(): boolean {
        return !!this.LocalStorage.obterTokenUsuario();
    }
}