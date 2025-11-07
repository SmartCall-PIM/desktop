using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmartCall.Models;
using SmartCall.Models.DTOs;
using SmartCall.Services;
using System.Linq;

namespace SmartCall.Forms
{
    public partial class FormAdicionarUsuario : Form
    {
        public FormAdicionarUsuario()
        {
            InitializeComponent();
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text.Trim();

            if (cmbCargo.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione um cargo.", "Campo Vazio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string cargoString = cmbCargo.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Por favor, preencha o nome, e-mail e senha.", "Campos Vazios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Desabilita o botão durante o processo
                btnSalvar.Enabled = false;

                // Criar requisição para a API
                var request = new RegisterRequest
                {
                    FullName = nome,
                    Email = email,
                    Password = senha,
                    ConfirmPassword = senha  // Usar a mesma senha para confirmação
                };

                // Fazer requisição para criar usuário
                var response = await ApiService.PostAsync<LoginResponse>("auth/register", request);

                if (response == null)
                {
                    MessageBox.Show("Erro ao conectar com o servidor.", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSalvar.Enabled = true;
                    return;
                }

                if (!response.Success)
                {
                    string errorMsg = response.Message ?? "Erro ao cadastrar usuário";
                    if (response.Errors != null && response.Errors.Count > 0)
                    {
                        errorMsg += "\n" + string.Join("\n", response.Errors);
                    }
                    MessageBox.Show(errorMsg, "Erro ao Cadastrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSalvar.Enabled = true;
                    return;
                }

                // Se o usuário foi criado com sucesso, atualizar o cargo se necessário
                if (cargoString != "Usuário" && !string.IsNullOrEmpty(response.User?.Id))
                {
                    var updateRequest = new UpdateUserRequest
                    {
                        Email = email,
                        FullName = nome,
                        Role = cargoString
                    };

                    await ApiService.PutAsync<ApiResponse>($"users/{response.User.Id}", updateRequest);
                }

                MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpar campos
                txtNome.Clear();
                txtEmail.Clear();
                txtSenha.Clear();
                cmbCargo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar usuário: " + ex.Message, "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSalvar.Enabled = true;
            }
        }

        private void FormAdicionarUsuario_Load(object sender, EventArgs e)
        {
            cmbCargo.Items.Add("Usuário");
            cmbCargo.Items.Add("Técnico");
            cmbCargo.Items.Add("Administrador");

            cmbCargo.SelectedIndex = 0;
        }
    }


}
