import { FormControl, AbstractControl, FormGroup, ValidatorFn } from '@angular/forms';

export class CustomValidator {
    static SenhasConferem(senha: string, senhaConfirm: string): ValidatorFn {
    return (ac: AbstractControl) =>{
        const senhaForm = ac.get(senha);
        const senhaConfirmForm = ac.get(senhaConfirm);
        if (senhaConfirmForm!.errors && !senhaConfirmForm!.errors?.['senhasNaoConferem']) {
            return null;
        }
        if (senhaForm!.value !== senhaConfirmForm!.value) {
            const error = { "senhasNaoConferem": true };
            senhaConfirmForm!.setErrors(error);
            return error;
            } else {
                senhaConfirmForm!.setErrors(null);
            return null;
            }
        }
    }
}