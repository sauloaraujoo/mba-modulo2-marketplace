import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { CadastroClienteComponent } from './cadastro-cliente/cadastro-cliente.component';
import { LoginComponent } from './login/login.component';
import { ContaAppComponent } from './conta.app.component';
import { EditarClienteComponent } from './editar-cliente/editar-cliente.component';

import { ContaRoutingModule } from './conta.route';
import { ContaService } from './services/conta.service';
import { ContaGuard } from './services/conta.guard';




@NgModule({
  declarations: [
    ContaAppComponent,
    CadastroClienteComponent, 
    LoginComponent,
    EditarClienteComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ContaRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    ContaService,
    ContaGuard
  ]
})
export class ContaModule { }