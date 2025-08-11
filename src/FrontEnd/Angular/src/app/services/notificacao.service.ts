import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export type TypeMessage = 'success' | 'error' | 'info' | 'warning';

export interface Toast {
  message?: string;
  duration: number;
  type: TypeMessage;
}

@Injectable({ providedIn: 'root' })
export class NotificacaoService {
  private toastSubject = new Subject<Toast>();
  public toast$ = this.toastSubject.asObservable();

  private mostrar(message: string, type: TypeMessage, duration: number = 5000) {
    this.toastSubject.next({message, type, duration});
  }

  mostrarSucesso(message: string, duration: number = 3000) {
    this.mostrar(message, 'success', duration);
  }

  mostrarErro(message: string, duration: number = 3000) {
    this.mostrar(message, 'error', duration);
  }

  mostrarInfo(message: string, duration: number = 3000) {
    this.mostrar(message, 'info', duration);
  }

  mostrarAviso(message: string, duration: number = 3000) {
    this.mostrar(message, 'warning', duration);
  }
}