import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControl, FormControlName, FormGroup, Validators } from '@angular/forms';
import { FormBaseComponent } from 'src/app/base-components/form-base.component';
import { Router } from '@angular/router';

import { Usuario } from '../models/usuario';
import { ContaService } from '../services/conta.service';
import { NotificacaoService } from 'src/app/services/notificacao.service';
import { CustomValidator } from 'src/app/utils/custom.validator';

@Component({
  selector: 'app-cadastro-cliente',
  templateUrl: './cadastro-cliente.component.html'
})
export class CadastroClienteComponent extends FormBaseComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  erros: any[] = [];
  cadastroForm!: FormGroup;
  usuario!: Usuario;

  constructor(private fb: FormBuilder,
    private contaService: ContaService,
    private router: Router,
    private notificacaoService: NotificacaoService) {

    super();

    this.validationMessages = {
      nome: {
        required: 'Informe o nome',
        minlength: 'A nome deve possuir no mínimo 2 caracteres',
        maxlength: 'A nome deve possuir no máximo 255 caracteres'
      },
      email: {
        required: 'Informe o e-mail',
        email: 'Email inválido'
      },
      senha: {
        required: 'Informe a senha',
        minlength: 'A senha deve possuir no mínimo 6 caracteres',
        maxlength: 'A senha deve possuir no máximo 15 caracteres'
      },
      confirmacaoSenha: {
        required: 'Informe a senha novamente',
        senhasNaoConferem: 'As senhas não conferem'
      }
    };

    super.configurarMensagensValidacaoBase(this.validationMessages);
  }
    
  ngAfterViewInit(): void {
    super.configurarValidacaoFormularioBase(this.formInputElements, this.cadastroForm);
  }

  ngOnInit(): void {
    this.cadastroForm = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(255)]],
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      confirmacaoSenha: ['', Validators.required]
    }, { validators:[
          CustomValidator.SenhasConferem('senha', 'confirmacaoSenha')
        ] 
      });
  }
    

  adicionarConta() {
    if (this.cadastroForm.dirty && this.cadastroForm.valid) {
      this.usuario = Object.assign({}, this.usuario, this.cadastroForm.value);

      this.contaService.registrarUsuario(this.usuario)
        .subscribe(
          sucesso => { this.processarSucesso(sucesso) },
          falha => { this.processarFalha(falha) }
        );

      this.mudancasNaoSalvas = false;
    }
  }
  
  processarSucesso(response: any) {
    this.cadastroForm.reset();
    this.erros = [];
    this.contaService.LocalStorage.salvarDadosLocaisUsuario(response);

    this.notificacaoService.mostrarSucesso('Login realizado com Sucesso!');
    
    this.router.navigate(['/produtos']);
    
  }

  processarFalha(fail: any){
    this.erros = fail.error.mensagens;
    this.notificacaoService.mostrarErro('Ocorreu um erro!');
  }
}
