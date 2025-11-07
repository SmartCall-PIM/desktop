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
    public partial class FormEditarUsuario : Form
    {

        private UserListResponse? usuarioParaEditar;

        public FormEditarUsuario()
        {
            InitializeComponent();
        }

        private void FormEditarUsuario_Load(object sender, EventArgs e)
        {

            cmbCargo.Items.Add("Usuário");
            cmbCargo.Items.Add("Técnico");     
            cmbCargo.Items.Add("Administrador");

            panelEdicao.Enabled = false;
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string busca = txtBusca.Text.Trim();
            if (string.IsNullOrWhiteSpace(busca))
            {
                MessageBox.Show("Por favor, digite um ID ou E-mail para buscar.", "Busca Vazia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnBuscar.Enabled = false;

                // Buscar todos os usuários da API
                var usuarios = await ApiService.GetAsync<List<UserListResponse>>("users");

                if (usuarios == null)
                {
                    MessageBox.Show("Erro ao conectar com o servidor.", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnBuscar.Enabled = true;
                    return;
                }

                // Procurar o usuário na lista
                usuarioParaEditar = usuarios.FirstOrDefault(u =>
                    u.Id.ToString() == busca ||
                    u.Email.Equals(busca, StringComparison.OrdinalIgnoreCase)
                );

                if (usuarioParaEditar != null)
                {
                    txtNome.Text = usuarioParaEditar.FullName;
                    txtEmail.Text = usuarioParaEditar.Email;
                    txtNovaSenha.Clear();

                    // Definir cargo
                    cmbCargo.SelectedItem = usuarioParaEditar.Role;

                    panelEdicao.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Usuário não encontrado.", "Busca Falhou", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    panelEdicao.Enabled = false;
                    usuarioParaEditar = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnBuscar.Enabled = true;
            }
        }

        private async void btnSalvarEdicao_Click(object sender, EventArgs e)
        {
            if (usuarioParaEditar == null) return;

            string novoNome = txtNome.Text.Trim();
            string novoEmail = txtEmail.Text.Trim();
            string novaSenha = txtNovaSenha.Text.Trim();

            if (string.IsNullOrWhiteSpace(novoNome) || string.IsNullOrWhiteSpace(novoEmail))
            {
                MessageBox.Show("O nome e o e-mail não podem ficar vazios.", "Campos Vazios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnSalvarEdicao.Enabled = false;

                // Preparar requisição de atualização
                var updateRequest = new UpdateUserRequest
                {
                    Email = novoEmail,
                    FullName = novoNome,
                    Role = cmbCargo.SelectedItem?.ToString()
                };

                // Atualizar usuário via API
                var response = await ApiService.PutAsync<ApiResponse>($"users/{usuarioParaEditar.Id}", updateRequest);

                if (response == null)
                {
                    MessageBox.Show("Erro ao conectar com o servidor.", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSalvarEdicao.Enabled = true;
                    return;
                }

                // Atualizar senha se fornecida (nota: precisa de endpoint específico no backend)
                if (!string.IsNullOrWhiteSpace(novaSenha))
                {
                    MessageBox.Show("Aviso: A alteração de senha ainda não está implementada no backend.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                MessageBox.Show("Usuário atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpar formulário
                panelEdicao.Enabled = false;
                txtBusca.Clear();
                txtNome.Clear();
                txtEmail.Clear();
                txtNovaSenha.Clear();
                usuarioParaEditar = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar alterações: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSalvarEdicao.Enabled = true;
            }
        }

    }
}
