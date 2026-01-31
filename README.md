# Plataforma de Marcenaria - API .NET

Sistema para conectar marceneiros e clientes de forma organizada, segura e acessível.

## Sobre o Projeto

A plataforma conecta clientes que desejam móveis planejados com marceneiros qualificados, simplificando o processo de orçamento e contratação. Oferecemos uma plataforma segura e transparente para que ambas as partes possam se conectar e realizar projetos com confiança.

### Principais Funcionalidades

- Cadastro e autenticação de usuários (clientes, vendedores e marceneiros)
- Solicitação de orçamentos
- Agendamento de visitas técnicas
- Sistema de lances para projetos
- Gerenciamento de projetos
- Avaliações e feedback

## Tecnologias Utilizadas

- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core 8.0
- SQL Server
- JWT Authentication
- Swagger/OpenAPI para documentação
- BCrypt para hash de senhas

## Configuração do Ambiente

### Pré-requisitos

- .NET 8.0 SDK
- SQL Server (ou SQL Server Express)
- Visual Studio 2022 ou VS Code (recomendado)

### Configuração do Banco de Dados

1. Crie um banco de dados SQL Server chamado `five_marcenaria`
2. Configure a connection string no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=five_marcenaria;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;Encrypt=True;"
  }
}
```

### Executando o Projeto

```bash
# Clone o repositório
git clone [URL_DO_REPOSITÓRIO]

# Entre no diretório
cd PlataformaMarcenaria.API.NET

# Restaure as dependências
dotnet restore

# Execute as migrations (se necessário)
dotnet ef database update

# Execute o projeto
dotnet run
```

A API estará disponível em:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

## Documentação da API

A documentação da API está disponível através do Swagger UI em:
```
http://localhost:5000/swagger-ui.html
```

ou

```
https://localhost:5001/swagger-ui.html
```

## Estrutura do Projeto

```
PlataformaMarcenaria.API.NET/
├── Controllers/          # Controladores da API
├── Data/                 # DbContext e configurações do banco
├── DTOs/                 # Data Transfer Objects
├── Entities/             # Entidades do domínio
├── Exceptions/           # Exceções customizadas
├── Middleware/           # Middlewares customizados
├── Repositories/         # Repositórios e interfaces
├── Security/             # Configurações de segurança e JWT
├── Services/             # Serviços de negócio
├── appsettings.json      # Configurações da aplicação
└── Program.cs            # Ponto de entrada da aplicação
```

## Endpoints Principais

### Autenticação
- `POST /api/auth/register` - Registrar novo usuário
- `POST /api/auth/login` - Login e obter token JWT

### Usuários
- `GET /api/users` - Listar todos os usuários
- `GET /api/users/{id}` - Obter usuário por ID
- `POST /api/users` - Criar usuário (público)
- `PUT /api/users/{id}` - Atualizar usuário
- `DELETE /api/users/{id}` - Desativar usuário

### Orçamentos
- `GET /api/budget-requests` - Listar orçamentos
- `GET /api/budget-requests/{id}` - Obter orçamento por ID
- `POST /api/budget-requests` - Criar orçamento
- `PATCH /api/budget-requests/{id}/status` - Atualizar status
- `DELETE /api/budget-requests/{id}` - Excluir orçamento

### Lances
- `GET /api/bids` - Listar lances
- `POST /api/bids` - Criar lance
- `POST /api/bids/{id}/accept` - Aceitar lance
- `POST /api/bids/{id}/reject` - Rejeitar lance

### Visitas
- `GET /api/visits` - Listar visitas
- `POST /api/visits` - Agendar visita
- `PATCH /api/visits/{id}/status` - Atualizar status da visita

### Endereços
- `GET /api/addresses` - Listar endereços
- `POST /api/addresses` - Criar endereço
- `GET /api/addresses/user/{userId}` - Endereços por usuário

## Autenticação

A API utiliza JWT (JSON Web Tokens) para autenticação. Após fazer login, você receberá um token que deve ser incluído no header de todas as requisições autenticadas:

```
Authorization: Bearer {seu_token_jwt}
```

## Contribuição

1. Faça um Fork do projeto
2. Crie uma Branch para sua Feature (`git checkout -b feature/AmazingFeature`)
3. Faça o Commit das suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Faça o Push para a Branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

