using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmartCall.Models; // <-- ADICIONE ESTE (para o DbContext e Usuario)
using SmartCall.Models.DTOs;
using SmartCall.Services;
using System.Linq;      // <-- ADICIONE ESTE (para .Select() e .ToList())

namespace SmartCall.Forms
{
    public partial class FormVerUsuarios : Form
    {
        public FormVerUsuarios()
        {
            InitializeComponent();
        }

        public async Task CarregarUsuariosAsync()
        {
            try
            {
                // Busca usuários da API
                var usuarios = await ApiService.GetAsync<List<UserListResponse>>("users");

                if (usuarios == null || usuarios.Count == 0)
                {
                    MessageBox.Show("Nenhum usuário encontrado.",
                                    "Informação",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    dgvUsuarios.DataSource = null;
                    return;
                }

                // Preparar dados para exibição
                var listaParaExibir = usuarios.Select(u => new
                {
                    ID = u.Id,
                    Nome = u.FullName,
                    Email = u.Email,
                    Cargo = u.Role
                }).ToList();

                dgvUsuarios.DataSource = listaParaExibir;

                if (dgvUsuarios.Columns.Count > 0)
                {
                    dgvUsuarios.Columns["ID"].HeaderText = "ID";
                    dgvUsuarios.Columns["Nome"].HeaderText = "Nome Completo";
                    dgvUsuarios.Columns["Email"].HeaderText = "E-mail";
                    dgvUsuarios.Columns["Cargo"].HeaderText = "Nível de Acesso";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar usuários: " + ex.Message,
                                "Erro de Conexão",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private async void FormVerUsuarios_Load_1(object sender, EventArgs e)
        {
            await CarregarUsuariosAsync();
        }
    }
}