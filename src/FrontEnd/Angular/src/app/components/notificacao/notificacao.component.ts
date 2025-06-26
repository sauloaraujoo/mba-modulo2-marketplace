import { Component, OnInit } from '@angular/core';
import { NotificacaoService, Toast } from 'src/app/services/notificacao.service';

@Component({
  selector: 'app-notificacao',
  templateUrl: './notificacao.component.html',
  styleUrls: ['./notificacao.component.css']
})
export class NotificacaoComponent implements OnInit {
    toasts: Toast[] = [];
    visible = false;
    idTimeout: any;

    constructor(public notificacaoService: NotificacaoService) {}
    
    ngOnInit(): void {
        this.notificacaoService.toast$.subscribe((toast) => {
            this.toasts.push(toast);
            this.visible = true;
            this.idTimeout = setTimeout(() => {
                this.visible = false;
                this.toasts.shift();
            }, toast.duration);
        });
    }

    onmouseenter() {
        clearTimeout(this.idTimeout); // Pausa o temporizador
    }
    onmouseleave() {
        this.idTimeout = setTimeout(() => this.fechar(), 1000); // Reinicia o temporizador ao sair do toast
    }

    fechar() {
        this.visible = false;
    }
}