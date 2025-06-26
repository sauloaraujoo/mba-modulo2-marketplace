export class LocalStorageUtils {
    
    public obterUsuario() {
        const user = localStorage.getItem('lojamba.user');
        return user ? JSON.parse(user) : null;
    }

    public salvarDadosLocaisUsuario(response: any) {
        this.salvarTokenUsuario(response.accessToken);
        this.salvarUsuario(response.userToken);
    }

    public limparDadosLocaisUsuario() {
        localStorage.removeItem('lojamba.token');
        localStorage.removeItem('lojamba.user');
    }

    public obterTokenUsuario(): string | null {
        return localStorage.getItem('lojamba.token');
    }

    public salvarTokenUsuario(token: string) {
        localStorage.setItem('lojamba.token', token);
    }

    public salvarUsuario(user: string) {
        localStorage.setItem('lojamba.user', JSON.stringify(user));
    }

}