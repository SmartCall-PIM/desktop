# Guia Rápido - Testando a Integração Desktop + Backend

## Passo 1: Iniciar o Backend

Abra um terminal e execute:

```bash
cd c:\Users\Micael\Desktop\workspace\web\backend
dotnet run
```

Aguarde até ver a mensagem indicando que o servidor está rodando (geralmente na porta 5000 ou 5126).

## Passo 2: Criar um Usuário Administrador (Se Ainda Não Existir)

### Opção A: Via API diretamente

Use um cliente HTTP como Postman ou faça via PowerShell:

```powershell
$body = @{
    email = "admin@smartcall.com"
    password = "Admin@123"
    fullName = "Administrador"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/auth/register" -Method Post -Body $body -ContentType "application/json"
```

Depois, atualize o cargo para Administrador (você precisará do ID do usuário retornado):

```powershell
$token = "SEU_TOKEN_AQUI"  # Use o token retornado do registro
$body = @{
    email = "admin@smartcall.com"
    fullName = "Administrador"
    role = "Administrador"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/users/USER_ID_AQUI" -Method Put -Body $body -ContentType "application/json" -Headers @{Authorization="Bearer $token"}
```

### Opção B: Via aplicação web ou mobile (se já tiver usuários)

Use a interface web ou mobile para criar um usuário e depois atualize o cargo no banco de dados diretamente.

## Passo 3: Verificar a Porta do Backend

No terminal onde o backend está rodando, verifique em qual porta ele iniciou. Normalmente você verá algo como:

```
Now listening on: http://localhost:5000
```

Se for diferente de 5000, edite o arquivo `desktop\SmartCall\appsettings.json`:

```json
{
  "ApiBaseUrl": "http://localhost:PORTA_CORRETA"
}
```

## Passo 4: Iniciar a Aplicação Desktop

Abra outro terminal e execute:

```bash
cd c:\Users\Micael\Desktop\workspace\desktop\SmartCall
dotnet run
```

Ou compile e execute o executável:

```bash
cd c:\Users\Micael\Desktop\workspace\desktop\SmartCall
dotnet build
.\bin\Debug\net9.0-windows\SmartCall.exe
```

## Passo 5: Fazer Login

1. Na tela de login, digite:
   - **Email**: admin@smartcall.com (ou o email que você criou)
   - **Senha**: Admin@123 (ou a senha que você definiu)

2. Clique em "Login"

3. Se tudo estiver correto, a tela deve expandir mostrando as opções de gerenciamento

## Passo 6: Testar Funcionalidades

### Visualizar Usuários
- Clique no menu "Usuário" ou no ícone correspondente
- A lista de usuários deve carregar automaticamente

### Adicionar Usuário
- Clique em "Novo Usuário"
- Preencha os campos: Nome, Email, Senha, Cargo
- Clique em "Salvar"
- Verifique se o usuário aparece na lista

### Editar Usuário
- Clique em "Editar Usuário"
- Digite o ID ou Email do usuário
- Clique em "Buscar"
- Modifique os campos desejados
- Clique em "Salvar Edição"

### Deletar Usuário
- Clique em "Eliminar Usuário"
- Digite o ID ou Email do usuário
- Clique em "Buscar"
- Clique em "Confirmar Exclusão"
- Confirme a ação

## Troubleshooting

### Erro: "Erro ao conectar com o servidor"
**Causa**: O backend não está rodando ou está em uma porta diferente
**Solução**: 
1. Verifique se o backend está rodando
2. Confirme a porta no appsettings.json
3. Verifique se não há firewall bloqueando

### Erro: "Você não tem permissão de Administrador"
**Causa**: O usuário não tem o cargo "Administrador"
**Solução**: 
1. Verifique o cargo do usuário no banco de dados
2. Use um usuário com cargo correto
3. Atualize o cargo via API se necessário

### Erro: "Usuário ou senha incorretos"
**Causa**: Credenciais inválidas
**Solução**: 
1. Verifique o email e senha
2. Certifique-se de que o usuário existe no banco
3. Se necessário, crie um novo usuário

### Nenhum usuário aparece na lista
**Causa**: Não há usuários cadastrados ou erro na API
**Solução**: 
1. Verifique o console do backend para erros
2. Teste o endpoint diretamente: `GET http://localhost:5000/api/users`
3. Crie alguns usuários para teste

## Verificando os Logs

### Backend
Os logs aparecem no terminal onde você executou `dotnet run`

### Desktop
Erros são exibidos em caixas de diálogo do Windows

## Testando a API Manualmente

Você pode testar os endpoints diretamente:

```powershell
# Login
$loginBody = @{
    email = "admin@smartcall.com"
    password = "Admin@123"
} | ConvertTo-Json

$loginResponse = Invoke-RestMethod -Uri "http://localhost:5000/api/auth/login" -Method Post -Body $loginBody -ContentType "application/json"
$token = $loginResponse.token

# Listar usuários
Invoke-RestMethod -Uri "http://localhost:5000/api/users" -Method Get -Headers @{Authorization="Bearer $token"}
```

## Próximos Passos Após Teste Bem-Sucedido

1. ✅ Login funcionando
2. ✅ Listar usuários funcionando
3. ✅ Adicionar usuário funcionando
4. ✅ Editar usuário funcionando
5. ✅ Deletar usuário funcionando

Se todos os testes passarem, a integração está completa! 🎉

## Dicas de Desenvolvimento

- Use o Visual Studio ou VS Code com extensões C# para melhor experiência
- Mantenha o backend rodando enquanto desenvolve o desktop
- Consulte `INTEGRACAO.md` para detalhes técnicos da implementação
