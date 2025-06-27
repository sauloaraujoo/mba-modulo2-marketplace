import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const authGuard = (route: any, state: any) => {
  const router = inject(Router);

  const userData = localStorage.getItem('user');
  if (userData) {
    return true;
  }

  router.navigate(['/conta/login'], { queryParams: { returnUrl: state.url } });
  return false;
};