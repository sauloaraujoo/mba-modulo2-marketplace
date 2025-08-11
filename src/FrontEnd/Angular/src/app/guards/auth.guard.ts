import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const authGuard = (route: any, state: any) => {
  const router = inject(Router);

  const dadosUsuario = localStorage.getItem('lojamba.user');
  if (dadosUsuario) {
    return true;
  }

  router.navigate(['/conta/login'], { queryParams: { returnUrl: state.url } });
  return false;
};