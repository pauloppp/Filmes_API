namespace Filmes_API.Enums
{
    public enum StatusErro
    {
        Arquivo_Nao_Encontrado = 0,
        Arquivo_Vazio_Invalido = 1,
        Producer_Obrigatorio = 2,
        PreviousWin_Obrigatorio = 3,
        FollowingWin_Obrigatorio = 4,
        FollowingWin_Igual_PreviousWin = 5,
        FollowingWin_Menor_PreviousWin = 6,
        Arquivo_Processado_Com_Sucesso = 7,
        Arquivo_Incompleto = 8,
    }
}
