# Teste Dg Bar
### Teste da Empresa ClearSale
 
 

Primeiramente gostaria de agradecer a oportunidade de participar deste processo seletivo.

Abaixo algumas informações importantes de Decisões tomadas no projeto:

- Adotei o .Net Core para desenvolvimento da Prova;
- Adotei a Arquitetura Data-Driven para Criação da API,
- Decidir fazer o CRUD Básico das entidades que estavam explicitas no enunciado, e também das entidades e atributos que eram necessários para o funcionamento da aplicação;
- Documentação API (Swagger): http://localhost:5000/swagger
- Todo o sistema está parametrizado, até mesmo a opção de promoções foi criada uma solução para criar promoções que seja independente da número de produtos e quantidade de itens;
- Autentitação da Api: Foi utilizado JWT (JSON Web Token), no projeto foi implementado, Autenticação e também Autorização;
- Resiliência da API, Eu nunca havia implementado isso antes, fiz algumas pesquisas e deparei com a Biblioteca Polly que parece ser uma boa alternativa, como só tive tempo de desenvolver o projeto nas horas vagas, não consegui concluir essa etapa;
- Interface Web: Foi Criada Usando um Projeto de Aplicativo Web do .Net Core, mas ele foi todo escrito com (HTML / JS / CSS);

####  Possíveis pontos de evolução.
- Para facilitar a manutenção, seria interessante mais classes do tipo Service, para reduzir os códigos nas Controllers (Foi implementada algumas Classes services como demonstração);
- Poderia adicionar mais Unit Tests;
- A Resiliência da Api é algo que achei relevante ser adicionada;


#### Abaixo Links e as portas que estão sendo utilizadas, e o link para a Documentação da API

##### BackEnd (API)
http://localhost:5000/

##### Documentação API (Swagger)
http://localhost:5000/swagger


##### FrontEnd (Interface Web)
https://localhost:44309/

##### Dados para acesso a Interface Web:
```
usuario: fabio
senha: fabio
```

###### Banco de Dados
- Banco de dados .mdf encontra-se na pasta Banco de Dados, na raiz do projeto.
- Caso necessário, Criar um Banco LocalDB MSSQL com o comando abaixo:
```sql
create database TesteClearSale;
```


Crie a Estrutura do banco utilizando Migrations com o comando abaixo (Certifique-se que no terminal/console, você esteja no diretório do projeto.)
```bash
dotnet ef database update
```
###### Observação:
Como a Api e o FrontEnd estão na mesma solução, para executa-los verificar se Inicialização do Projeto está definida como "Vários projetos de inicialização".


Muito Obrigado!
