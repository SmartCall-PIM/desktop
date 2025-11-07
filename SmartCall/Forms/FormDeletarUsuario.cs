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
    public partial class FormDeletarUsuario : Form
    {

        private UserListResponse? usuarioParaDeletar;

        public FormDeletarUsuario()
        {
            InitializeComponent();
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
                usuarioParaDeletar = usuarios.FirstOrDefault(u =>
                    u.Id.ToString() == busca ||
                    u.Email.Equals(busca, StringComparison.OrdinalIgnoreCase)
                );

                if (usuarioParaDeletar != null)
                {
                    lblUsuarioEncontrado.Text = $"Nome: {usuarioParaDeletar.FullName} | E-mail: {usuarioParaDeletar.Email}";

                    btnDeletarConfirmar.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Usuário não encontrado.", "Busca Falhou", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    lblUsuarioEncontrado.Text = "(Nenhum usuário selecionado)";
                    btnDeletarConfirmar.Enabled = false;
                    usuarioParaDeletar = null;
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
        private async void btnDeletarConfirmar_Click(object sender, EventArgs e)
        {
            if (usuarioParaDeletar == null) return;

            var confirmacao = MessageBox.Show(
                $"Você tem certeza absoluta que deseja deletar permanentemente o usuário:\n\n" +
                $"{usuarioParaDeletar.FullName} ({usuarioParaDeletar.Email})\n\n" +
                $"Esta ação não pode ser desfeita.",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacao == DialogResult.No)
            {
                return;
            }

            try
            {
                btnDeletarConfirmar.Enabled = false;

                // Deletar usuário via API
                bool success = await ApiService.DeleteAsync($"users/{usuarioParaDeletar.Id}");

                if (success)
                {
                    MessageBox.Show("Usuário deletado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtBusca.Clear();
                    lblUsuarioEncontrado.Text = "(Nenhum usuário selecionado)";
                    btnDeletarConfirmar.Enabled = false;
                    usuarioParaDeletar = null;
                }
                else
                {
                    MessageBox.Show("Não foi possível deletar o usuário. Tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnDeletarConfirmar.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao deletar usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDeletarConfirmar.Enabled = true;
            }
        }

    }
}
