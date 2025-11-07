# SmartCall Desktop - Integração com Backend

## Descrição

A aplicação desktop SmartCall foi integrada com o backend através de API REST. Agora todas as operações de gerenciamento de usuários são feitas via chamadas HTTP ao backend.

## Mudanças Realizadas

### 1. Novo Serviço de API (`Services/ApiService.cs`)
- Gerencia todas as comunicações HTTP com o backend
- Suporta autenticação via token JWT
- Métodos para GET, POST, PUT, DELETE

### 2. DTOs Adicionados (`Models/DTOs/ApiDTOs.cs`)
- `LoginRequest` e `LoginResponse` - Para autenticação
- `UserListResponse` - Lista de usuários
- `UpdateUserRequest` - Atualização de usuários
- `CreateUserRequest` - Criação de usuários
- `RegisterRequest` - Registro de novos usuários

### 3. Formulários Atualizados

#### Login.cs
- Método `ValidarLoginAsync()` agora usa a API para autenticação
- Armazena token JWT para requisições futuras
- Verifica se o usuário tem permissão de Administrador

#### FormVerUsuarios.cs
- `CarregarUsuariosAsync()` busca usuários da API
- Exibe dados formatados no DataGridView

#### FormAdicionarUsuario.cs
- Cria usuários via endpoint `auth/register`
- Atualiza o cargo do usuário após criação se necessário

#### FormEditarUsuario.cs
- Busca usuários da API
- Atualiza informações via endpoint `users/{id}`

#### FormDeletarUsuario.cs
- Busca e deleta usuários via API
- Confirmação antes de deletar

### 4. Configuração
- Arquivo `appsettings.json` para configurar URL do backend
- Carregamento automático da configuração no `Program.cs`

## Como Usar

### 1. Configurar o Backend

Certifique-se de que o backend está rodando. Por padrão, ele deve estar em `http://localhost:5000`.

Para iniciar o backend:
```bash
cd web/backend
dotnet run
```

### 2. Configurar a URL da API (Opcional)

Se o backend estiver em uma porta diferente, edite o arquivo `appsettings.json`:

```json
{
  "ApiBaseUrl": "http://localhost:PORTA_DIFERENTE"
}
```

### 3. Executar o Desktop

```bash
cd desktop/SmartCall
dotnet run
```

### 4. Login

- Use um email e senha de um usuário com cargo "Administrador"
- O sistema validará as credenciais via API
- Após o login bem-sucedido, todas as operações usarão o token JWT

## Endpoints Utilizados

| Operação | Método | Endpoint | Descrição |
|----------|--------|----------|-----------|
| Login | POST | `/api/auth/login` | Autenticação de usuário |
| Listar Usuários | GET | `/api/users` | Lista todos os usuários |
| Criar Usuário | POST | `/api/auth/register` | Cria novo usuário |
| Atualizar Usuário | PUT | `/api/users/{id}` | Atualiza dados do usuário |
| Deletar Usuário | DELETE | `/api/users/{id}` | Remove usuário |

## Requisitos

- .NET 9.0
- Backend SmartCall rodando
- Conexão de rede com o backend

## Notas Importantes

1. **Autenticação**: Todas as requisições (exceto login) exigem autenticação via token JWT
2. **Permissões**: Apenas usuários com cargo "Administrador" podem fazer login no desktop
3. **Sincronização**: O desktop não usa mais banco de dados local - tudo é gerenciado via API
4. **Timeout**: As requisições têm timeout de 30 segundos

## Próximos Passos

- [ ] Implementar alteração de senha no backend e integrar com o desktop
- [ ] Adicionar indicador de loading durante operações assíncronas
- [ ] Implementar cache local para melhorar performance
- [ ] Adicionar logs de erro mais detalhados
- [ ] Implementar refresh automático do token JWT

## Troubleshooting

### Erro de Conexão
- Verifique se o backend está rodando
- Confirme a URL no `appsettings.json`
- Verifique firewall/antivírus

### Erro de Autenticação
- Certifique-se de que o usuário tem cargo "Administrador"
- Verifique se o backend está configurado corretamente

### Erro ao Criar Usuário
- O endpoint `auth/register` cria usuários com cargo padrão "Usuário"
- Para criar Admin/Técnico, o sistema primeiro cria e depois atualiza o cargo
