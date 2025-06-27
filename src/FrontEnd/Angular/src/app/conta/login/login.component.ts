import { Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { FormBaseComponent } from 'src/app/base-components/form-base.component';
import { ActivatedRoute, Router } from '@angular/router';

import { Usuario } from '../models/usuario';
import { ContaService } from '../services/conta.service';
import { NotificacaoService } from 'src/app/services/notificacao.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent extends FormBaseComponent implements OnInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];
  errors: any[] = [];
  loginForm!: FormGroup;
  usuario!: Usuario;

  returnUrl!: string;

  constructor(private fb: FormBuilder,
    private contaService: ContaService,
    private router: Router,
    private route: ActivatedRoute,
    private notificacaoService: NotificacaoService) {

      super();

      this.validationMessages = {
        email: {
          required: 'Informe o e-mail',
          email: 'Email inválido'
        },
        password: {
          required: 'Informe a senha',
          minlength: 'A senha deve possuir no mínimo 6 caracteres',
          maxlength: 'A senha deve possuir no máximo 15 caracteres'
        }
      };

      this.returnUrl = this.route.snapshot.queryParams['returnUrl'];

      super.configurarMensagensValidacaoBase(this.validationMessages);    
  }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]]
    });
  }

  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.loginForm);
  }

  login() {
    if (this.loginForm.dirty && this.loginForm.valid) {
      this.usuario = Object.assign({}, this.usuario, this.loginForm.value);

      this.contaService.login(this.usuario)
      .subscribe(
          sucesso => {this.processarSucesso(sucesso)},
          falha => {this.processarFalha(falha)}
      );
    }
  }

  processarSucesso(response: any) {
    this.loginForm.reset();
    this.errors = [];
    this.contaService.LocalStorage.salvarDadosLocaisUsuario(response);

    this.notificacaoService.showSuccess('Login realizado com Sucesso!');
    
    if (this.returnUrl) {
      this.router.navigate([this.returnUrl]);
    } else {
      this.router.navigate(['/produtos']);
    }
  }

  processarFalha(fail: any){
    this.errors = fail.error.mensagens;
    this.notificacaoService.showError('Ocorreu um erro!');
  }
}
