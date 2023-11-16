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
    public partial class AddUserForm : Form
    {
        // Declaraciones de los controles
        private TextBox txtUsername, txtPassword, txtNombre, txtApellido, txtEmail, txtTelefono;
        private ComboBox comboBoxRoles;
        private Button btnSave, btnCancel;
        private string connectionString = "server=44.201.241.190;database=proyecto2023;uid=root;pwd=89Vg3zaKAJJjKvCFDPmARPmjyMfV27Bk;";


        public AddUserForm()
        {
            InitializeComponent();
            InitializeFormControls();
        }

        private void InitializeFormControls()
        {
            int yPos = 10;

            // Método para crear un par de Label y TextBox para cada campo
            txtUsername = CreateTextbox("Username", yPos);
            txtPassword = CreateTextbox("Password", yPos += 30);
            txtNombre = CreateTextbox("Nombre", yPos += 30);
            txtApellido = CreateTextbox("Apellido", yPos += 30);
            txtEmail = CreateTextbox("Email", yPos += 30);
            txtTelefono = CreateTextbox("Telefono", yPos += 30);

            // ComboBox para Rol
            CreateLabel("Rol", yPos += 30);
            comboBoxRoles = new ComboBox { Left = 110, Top = yPos, Width = 170 };
            this.Controls.Add(comboBoxRoles);

            // Botones Guardar y Cancelar
            btnSave = new Button { Text = "Guardar", Left = 50, Top = yPos += 30, Width = 100 };
            btnSave.Click += new EventHandler(btnSave_Click);
            this.Controls.Add(btnSave);

            btnCancel = new Button { Text = "Cancelar", Left = 160, Top = yPos, Width = 100 };
            btnCancel.Click += (sender, e) => { this.Close(); };
            this.Controls.Add(btnCancel);

            // Cargar roles en el ComboBox (Este método debe ser implementado)
            CargarRoles();

            this.ClientSize = new Size(300, yPos + 50); // Ajustar tamaño del formulario
        }

        private TextBox CreateTextbox(string labelText, int yPos)
        {
            CreateLabel(labelText, yPos);
            TextBox textBox = new TextBox { Left = 110, Top = yPos, Width = 170 };
            this.Controls.Add(textBox);
            return textBox;
        }

        private void CreateLabel(string text, int yPos)
        {
            Label label = new Label { Text = text, Left = 20, Top = yPos + 3, Width = 80 };
            this.Controls.Add(label);
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
                    string query = "INSERT INTO usuarios (username, password, nombre, apellido, email, telefono, idRol) VALUES (@username, @password, @nombre, @apellido, @email, @telefono, @idRol)";

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

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuario agregado con éxito.");
                    this.Close(); // Cierra el formulario
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar usuario: " + ex.Message);
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
