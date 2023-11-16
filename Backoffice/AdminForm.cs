using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Backoffice
{
    public partial class AdminForm : Form
    {
        private TabControl tabControl;
        private TabPage tabUsuarios;
        private TabPage tabPaquetes;
        private TabPage tabLotes;
        private TabPage tabDepositos;
        private DataGridView dgvUsuarios;
        private DataGridView dgvPaquetes;
        private DataGridView dgvLotes;
        private DataGridView dgvDepositos;
        private string connectionString = "server=44.201.241.190;database=proyecto2023;uid=root;pwd=89Vg3zaKAJJjKvCFDPmARPmjyMfV27Bk;";


        public AdminForm()
        {
            InitializeComponent();
            InitializeAdminUI();
        }

        private void InitializeAdminUI()
        {
            // Configurar TabControl
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(tabControl);

            // Pestaña de Usuarios
            tabUsuarios = new TabPage("Usuarios");
            dgvUsuarios = CreateDataGridView();
            tabUsuarios.Controls.Add(dgvUsuarios);
            AddActionButtons(tabUsuarios, "Usuarios");
            tabControl.TabPages.Add(tabUsuarios);

            // Pestaña de Lotes
            tabLotes = new TabPage("Lotes");
            dgvLotes = CreateDataGridView();
            tabLotes.Controls.Add(dgvLotes);
            AddActionButtons(tabLotes, "Lotes");
            tabControl.TabPages.Add(tabLotes);

            // Pestaña de Paquetes
            tabPaquetes = new TabPage("Paquetes");
            dgvPaquetes = CreateDataGridView();
            tabPaquetes.Controls.Add(dgvPaquetes);
            AddActionButtons(tabPaquetes, "Paquetes");
            tabControl.TabPages.Add(tabPaquetes);

            // Pestaña de Depositos
            tabDepositos = new TabPage("Depositos");
            dgvDepositos = CreateDataGridView();
            tabDepositos.Controls.Add(dgvDepositos);
            AddActionButtons(tabDepositos, "Depositos");
            tabControl.TabPages.Add(tabDepositos);


            // Configuraciones adicionales y carga de datos aquí...
            RefreshTables();

        }

        private DataGridView CreateDataGridView()
        {
            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            // Configuraciones adicionales para DataGridView...
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ClearSelection();
            dgv.RowHeadersVisible = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.BackgroundColor = this.BackColor;
            return dgv;
        }

        private void LoadDataIntoDataGridView(DataGridView dgv, string query)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgv.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void AddActionButtons(TabPage tab, string tipo)
        {
            Panel panelButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };
            tab.Controls.Add(panelButtons);

            // Botón Agregar
            Button btnAdd = CreateButton("Agregar " + tipo);
            btnAdd.Click += (sender, e) => {
                if (tipo == "Usuarios")
                {
                    AddUserForm AddUserForm = new AddUserForm();
                    AddUserForm.Show();
                }
                if (tipo == "Lotes")
                {
                    AddLoteForm AddLoteForm = new AddLoteForm();
                    AddLoteForm.Show();
                }

                if (tipo == "Depositos")
                {
                    AddDepositoForm AddDepositoForm = new AddDepositoForm();
                    AddDepositoForm.Show();
                }
                if (tipo == "Paquetes")
                {
                    AddPaquetesForm AddPaqueteForm = new AddPaquetesForm();
                    AddPaqueteForm.Show();
                }

                RefreshTables();
            };
            panelButtons.Controls.Add(btnAdd);

            // Botón Editar
            Button btnEdit = CreateButton("Editar " + tipo);
            btnEdit.Click += (sender, e) => {
                if (tipo == "Usuarios")
                {
                    if (dgvUsuarios.CurrentRow == null)
                    {
                        MessageBox.Show("Por favor, selecciona un usuario para editar.");
                        return;
                    }

                    int userId = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["idUser"].Value);

                    EditUserForm editUserForm = new EditUserForm(userId);
                    editUserForm.ShowDialog();
                }
                if (tipo == "Lotes")
                {
                    if (dgvLotes.CurrentRow == null)
                    {
                        MessageBox.Show("Por favor, selecciona un lote para editar.");
                        return;
                    }

                    int loteId = Convert.ToInt32(dgvLotes.CurrentRow.Cells["idLote"].Value);

                    EditLoteForm EditLoteForm = new EditLoteForm(loteId);
                    EditLoteForm.ShowDialog();
                }
                if (tipo == "Depositos")
                {
                    if (dgvDepositos.CurrentRow == null)
                    {
                        MessageBox.Show("Por favor, selecciona un deposito para editar.");
                        return;
                    }

                    int depositoId = Convert.ToInt32(dgvDepositos.CurrentRow.Cells["idDeposito"].Value);

                    EditDepositoForm EditDepositoForm = new EditDepositoForm(depositoId);
                    EditDepositoForm.ShowDialog();
                }
                if (tipo == "Paquetes")
                {
                    if (dgvPaquetes.CurrentRow == null)
                    {
                        MessageBox.Show("Por favor, selecciona un paquete para editar.");
                        return;
                    }

                    int paqueteId = Convert.ToInt32(dgvPaquetes.CurrentRow.Cells["idPaquete"].Value);

                    EditPaqueteForm EditPaqueteForm = new EditPaqueteForm(paqueteId);
                    EditPaqueteForm.ShowDialog();
                }


                RefreshTables();

            };
            panelButtons.Controls.Add(btnEdit);

            // Botón Eliminar
            Button btnDelete = CreateButton("Eliminar " + tipo);
            btnDelete.Click += (sender, e) => { this.btnDelete_Click(sender, e, tipo); };
            panelButtons.Controls.Add(btnDelete);

            // Botón de Refresh
            Button btnRefresh = CreateButton("Refresh " + tipo);
            btnRefresh.Dock = DockStyle.Right;
            btnRefresh.Click += (sender, e) => { RefreshTables(); };
            panelButtons.Controls.Add(btnRefresh);

            // Botón de Logout
            Button btnLogout = CreateButton("Logout");
            btnLogout.Dock = DockStyle.Right;
            btnLogout.Click += (sender, e) => {
                Form1 Form1 = new Form1();
                this.Hide(); 
                Form1.Show();
            };
            panelButtons.Controls.Add(btnLogout);

        }

        private Button CreateButton(string text)
        {
            return new Button
            {
                Text = text,
                Dock = DockStyle.Left,
                Width = 100
            };
        }


        private void btnDelete_Click(object sender, EventArgs e, string tipo)
        {
            if(tipo == "Usuarios")
            {
                if (dgvUsuarios.CurrentRow == null)
                {
                    MessageBox.Show("Por favor, selecciona un usuario para eliminar.");
                    return;
                }

                int userId = Convert.ToInt32(dgvUsuarios.CurrentRow.Cells["idUser"].Value);

                if (MessageBox.Show("¿Estás seguro de que quieres eliminar este usuario? "+ userId, "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DeleteUserFromDatabase(userId);
                    RefreshTables();
                }
            }

            if (tipo == "Lotes")
            {
                if (dgvLotes.CurrentRow == null)
                {
                    MessageBox.Show("Por favor, selecciona un lote para eliminar.");
                    return;
                }

                int loteId = Convert.ToInt32(dgvLotes.CurrentRow.Cells["idLote"].Value);

                if (MessageBox.Show("¿Estás seguro de que quieres eliminar este lote? " + loteId, "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DeleteLoteFromDatabase(loteId);
                    RefreshTables();
                }
            }

            if (tipo == "Paquetes")
            {
                if (dgvPaquetes.CurrentRow == null)
                {
                    MessageBox.Show("Por favor, selecciona un paquete para eliminar.");
                    return;
                }

                int paqueteId = Convert.ToInt32(dgvPaquetes.CurrentRow.Cells["idPaquete"].Value);

                if (MessageBox.Show("¿Estás seguro de que quieres eliminar este paquete? " + paqueteId, "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DeletePaqueteFromDatabase(paqueteId);
                    RefreshTables();
                }
            }

            if (tipo == "Depositos")
            {
                if (dgvDepositos.CurrentRow == null)
                {
                    MessageBox.Show("Por favor, selecciona un deposito para eliminar.");
                    return;
                }

                int depositoId = Convert.ToInt32(dgvDepositos.CurrentRow.Cells["idDeposito"].Value);

                if (MessageBox.Show("¿Estás seguro de que quieres eliminar este deposito? " + depositoId, "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DeleteDepositoFromDatabase(depositoId);
                    RefreshTables();
                }
            }

        }

        // Métodos adicionales para manejar eventos y lógica de negocio aquí...



        private void DeleteUserFromDatabase(int userId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM usuarios WHERE idUser = @userId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuario eliminado con éxito.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar usuario: " + ex.Message);
                }
            }
        }

        private void DeletePaqueteFromDatabase(int paqueteId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM paquetes WHERE idPaquete = @userId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", paqueteId);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Paquete eliminado con éxito.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar paquete: " + ex.Message);
                }
            }
        }

        private void DeleteLoteFromDatabase(int loteId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM lotes WHERE idLote = @userId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", loteId);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Lote eliminado con éxito.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar Lote: " + ex.Message);
                }
            }
        }
        private void DeleteDepositoFromDatabase(int depositoId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM depositos WHERE idDeposito = @userId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", depositoId);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Deposito eliminado con éxito.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar Deposito: " + ex.Message);
                }
            }
        }

        public void RefreshTables()
        {
            LoadDataIntoDataGridView(dgvUsuarios, "SELECT u.idUser, u.username as 'Username', u.nombre as 'Nombre', u.apellido as 'Apellido', u.email as 'Correo', u.telefono as 'Telefono', rol.descRol as 'Rol' FROM usuarios u INNER JOIN roles rol ON u.idRol = rol.idRol;\r\n");
            LoadDataIntoDataGridView(dgvPaquetes, "SELECT idPaquete, descLote as 'Lote', descripcion as 'Descripcion', peso as 'Peso', direccionDestino as 'Direccion', username as 'Propietario', descStatus as 'Estado', descDeposito as 'Deposito' FROM vwPaquetes");
            LoadDataIntoDataGridView(dgvLotes, "SELECT * FROM lotes");
            LoadDataIntoDataGridView(dgvDepositos, "SELECT * FROM depositos");
    }

    }

}
