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
    public partial class EditUserForm : Form
    {
        private TextBox txtUsername, txtPassword, txtNombre, txtApellido, txtEmail, txtTelefono;
        private ComboBox comboBoxRoles;
        private Button btnSave, btnCancel;
        private int userId;
        private string connectionString = "server=44.201.241.190;database=proyecto2023;uid=root;pwd=89Vg3zaKAJJjKvCFDPmARPmjyMfV27Bk;";

        public EditUserForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            InitializeFormControls();
        }

        private void InitializeFormControls()
        {
            // Posicionamiento inicial de los controles
            int yPos = 20;

            txtUsername = CreateTextbox(yPos, "Username");
            txtPassword = CreateTextbox(yPos += 30, "Password");
            txtNombre = CreateTextbox(yPos += 30, "Nombre");
            txtApellido = CreateTextbox(yPos += 30, "Apellido");
            txtEmail = CreateTextbox(yPos += 30, "Email");
            txtTelefono = CreateTextbox(yPos += 30, "Teléfono");
            comboBoxRoles = CreateComboBox(yPos += 30, "Rol");

            btnSave = CreateButton(yPos += 30, "Guardar");
            btnSave.Click += new EventHandler(btnSave_Click);

            btnCancel = CreateButton(yPos, "Cancelar");
            btnCancel.Click += (sender, e) => { this.Close(); };
            CargarRoles();
            LoadUserData();
            

            this.ClientSize = new Size(300, yPos + 50); // Ajustar tamaño del formulario
        }

        private TextBox CreateTextbox(int yPos, string labelText)
        {
            Label label = new Label { Text = labelText, Left = 20, Top = yPos, Width = 80 };
            TextBox textBox = new TextBox { Left = 110, Top = yPos, Width = 170 };
            this.Controls.Add(label);
            this.Controls.Add(textBox);
            return textBox;
        }

        private ComboBox CreateComboBox(int yPos, string labelText)
        {
            Label label = new Label { Text = labelText, Left = 20, Top = yPos, Width = 80 };
            ComboBox comboBox = new ComboBox { Left = 110, Top = yPos, Width = 170 };
            this.Controls.Add(label);
            this.Controls.Add(comboBox);
            return comboBox;
        }

        private Button CreateButton(int yPos, string text)
        {
            Button button = new Button { Text = text, Left = 110, Top = yPos, Width = 80 };
            this.Controls.Add(button);
            return button;
        }

        private void LoadUserData()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT username, password, nombre, apellido, email, telefono, idRol FROM usuarios WHERE idUser = @userId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtUsername.Text = reader["username"].ToString();
                                txtPassword.Text = reader["password"].ToString(); // Considera no mostrar la contraseña
                                txtNombre.Text = reader["nombre"].ToString();
                                txtApellido.Text = reader["apellido"].ToString();
                                txtEmail.Text = reader["email"].ToString();
                                txtTelefono.Text = reader["telefono"].ToString();
                                comboBoxRoles.SelectedValue = reader["idRol"];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos del usuario: " + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string email = txtEmail.Text;
            string telefono = txtTelefono.Text;
            int rolId = Convert.ToInt32(comboBoxRoles.SelectedValue);


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE usuarios SET username = @username, password = @password, nombre = @nombre, apellido = @apellido, email = @email, telefono = @telefono, idRol = @idRol WHERE idUser = @idUser;";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Asegúrate de encriptar la contraseña antes de guardarla
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@apellido", apellido);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@telefono", telefono);
                        command.Parameters.AddWithValue("@idRol", rolId);
                        command.Parameters.AddWithValue("@idUser", userId);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuario editado con éxito.");
                    this.Close(); // Cierra el formulario
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al editar usuario: " + ex.Message);
                }

            }
        }

            private void CargarRoles()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT idRol, descRol FROM roles";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBoxRoles.DisplayMember = "descRol";
                    comboBoxRoles.ValueMember = "idRol";
                    comboBoxRoles.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar roles: " + ex.Message);
                }
            }
        }
    }
}
