# Filmes_API

## Instruções para uso da API Filmes:

### 1. Executar o projeto de testes [Filmes_API.Tests]
       1.1 Caso testes passem (OK).
       
           1.1.1 Executar projeto principal [Filmes_API]

       1.2 Caso testes não passem (Não OK).    

           1.2.1 Verificar resultados dos testes.

           1.2.2 Corrigir informações no arquivo de dados externo "Filmes.csv".


 ### 2. Executar o projeto principal [Filmes_API]
        2.1 Obter lista de possíveis erros no processamento do arquivo de dados externos.
            EndPoint: [GET../api/v1/Erros/GetErros].

        2.2 Se existirem erros de processamento, efetuar as correções conforme resultado do Log de erros.

        2.3 Executar o projeto principal novamente para reprocessar as informações corrigidas no arquivo "Filmes.csv".

        2.4 Obter o produtor com o maior intervalo entre dois prêmios consecutivos, e o que obteve dois prêmios mais rápido, seguindo a especificação de formato definida em requisitos da API.
            EndPoint: [GET../api/v1/Filmes/GetIndicados].

        2.5 Obter e movimentar as informações/dados sobre os filmes através dos demais endPoints disponíveis.
