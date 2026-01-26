# 🚀 SmartList Web API

Esta é uma Web API robusta construída com **ASP.NET Core 8**, seguindo os princípios da **Clean Architecture** e as melhores práticas de mercado.

## 🏗️ Estrutura do Projeto

O projeto está organizado da seguinte forma:

* **src/SmartList.Domain**: Contém as entidades, interfaces de repositórios e regras de negócio puras.
* **src/SmartList.Application**: Contém os casos de uso, DTOs, Mapeamentos e Validações (FluentValidation).
* **src/SmartList.Infrastructure**: Implementação do Entity Framework, Repositórios e integrações externas.
* **src/SmartList.API**: Ponto de entrada da aplicação, Controllers, Middlewares e configurações de Segurança (JWT).
* **tests/SmartList.Tests**: Testes unitários utilizando xUnit e Moq.

---

## 🛠️ Tecnologias Utilizadas

* **C# 12 / .NET 9**
* **Entity Framework Core** (SQL Server)
* **FluentValidation** (Validação de entrada)
* **AutoMapper** (Mapeamento de objetos)
* **JWT (JSON Web Token)** (Autenticação)
* **Swagger/OpenAPI** (Documentação)
* **Docker & Docker Compose** (Containerização)

---

## 🚦 Como Iniciar

### 1. Pré-requisitos

* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* Ferramenta de Migrations: `dotnet tool install --global dotnet-ef`

### 2. Configuração de Segurança (Chave JWT)

Para rodar localmente, você deve configurar uma chave secreta no seu gerenciador de segredos local:

```bash
cd src/SmartList.API
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "SuaChaveSuperSecretaDePeloMenos32Caracteres"

```

### 3. Executando com Docker (Recomendado)

A forma mais rápida de subir a API e o Banco de Dados SQL Server:

```bash
docker-compose up -d

```

A API estará disponível em: `http://localhost:5000/swagger`

### 4. Executando Via CLI (Desenvolvimento)

Se preferir rodar sem Docker, configure a connection string no `appsettings.Development.json` e execute:

```bash
# Rodar Migrations para criar o banco
dotnet ef database update --project src/SmartList.Infrastructure --startup-project src/SmartList.API

# Iniciar a API
dotnet run --project src/SmartList.API

```

---

## 🧪 Testes

Para garantir a qualidade e as regras de negócio:

```bash
dotnet test

```

---

## 🔒 Endpoints Principais

* `POST /api/auth/login`: Autentica e gera o Token JWT.
* `GET /api/produtos`: Lista produtos (Requer Header `Authorization: Bearer {token}`).
* `POST /api/produtos`: Cria um novo produto (Requer perfil Admin).

---

## 📄 Notas de Implementação

* **Global Exception Handling**: Erros são capturados centralizadamente e retornam um JSON padronizado.
* **Repository Pattern**: Acesso a dados desacoplado via interfaces.
* **Validations**: Validações automáticas antes de atingir a camada de serviço.
