# Filmes_API

## Considerações Gerais:

Projeto API_Filmes desenvolvido inicialmente em estrutura monolítica por se tratar de processamento simples e objetivo.

Para outras necessidades, redefinir arquitetura.

A aplicação fará a leitura/processamento de todos os arquivos existentes na pasta [Data/Principal].


## Instruções para uso da API Filmes:

### 1. Executar o projeto de testes [Filmes_API.Tests]

       1.1 Executar Filmes_API.Context (Verifica regras para o banco de dados).

       1.2 Executar Filmes_API.External (Verifica carga dos dados externos "Arquivos.csv").


 ### 2. Executar o projeto principal [Filmes_API]
        2.1 Obter lista de possíveis erros no processamento do arquivo de dados externos.
            EndPoint: [GET../api/v1/Erros/GetErros].

        2.2 Se existirem erros de processamento, efetuar as correções conforme resultado do Log de erros.

        2.3 Executar o projeto principal novamente para reprocessar as informações corrigidas nos arquivos "...Filmes.csv".

        2.4 Obter o produtor com o maior intervalo entre dois prêmios consecutivos, e o que obteve dois prêmios mais rápido, seguindo a especificação de formato definida em requisitos da API.
            EndPoint: [GET../api/v1/Filmes/GetIndicados].

        2.5 Obter e movimentar as informações/dados sobre os filmes através dos demais endPoints disponíveis.
