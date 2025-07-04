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

  private show(message: string, type: TypeMessage, duration: number = 5000) {
    this.toastSubject.next({message, type, duration});
  }

  showSuccess(message: string, duration: number = 3000) {
    this.show(message, 'success', duration);
  }

  showError(message: string, duration: number = 3000) {
    this.show(message, 'error', duration);
  }

  showInfo(message: string, duration: number = 3000) {
    this.show(message, 'info', duration);
  }

  showWarning(message: string, duration: number = 3000) {
    this.show(message, 'warning', duration);
  }
}