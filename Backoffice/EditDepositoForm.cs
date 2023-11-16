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
    public partial class EditDepositoForm : Form
    {
        private TextBox txtDesc, txtDireccion, txtCalle, txtPuerta;
        private Button btnSave, btnCancel;
        private int depositoId;
        private string connectionString = "server=44.201.241.190;database=proyecto2023;uid=root;pwd=89Vg3zaKAJJjKvCFDPmARPmjyMfV27Bk;";


        public EditDepositoForm( int depositoId)
        {
            InitializeComponent();
            this.depositoId = depositoId;
            InitializeFormControls();
        }

        private void InitializeFormControls()
        {
            int yPos = 10;

            // Método para crear un par de Label y TextBox para cada campo
            txtDesc = CreateTextbox("Desc Deposito", yPos);
            txtDireccion = CreateTextbox("Direccion", yPos += 30);
            txtCalle = CreateTextbox("Calle", yPos += 30);
            txtPuerta = CreateTextbox("Puerta", yPos += 30);


            // Botones Guardar y Cancelar
            btnSave = new Button { Text = "Guardar", Left = 50, Top = yPos += 30, Width = 100 };
            btnSave.Click += new EventHandler(btnSave_Click);
            this.Controls.Add(btnSave);

            btnCancel = new Button { Text = "Cancelar", Left = 160, Top = yPos, Width = 100 };
            btnCancel.Click += (sender, e) => { this.Close(); };
            this.Controls.Add(btnCancel);

            LoadUserData();

            this.ClientSize = new Size(300, yPos + 50); // Ajustar tamaño del formulario
        }


        private void LoadUserData()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT descDeposito, direccion, calle, puerta FROM depositos WHERE idDeposito = @depositoId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@depositoId", depositoId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtDesc.Text = reader["descDeposito"].ToString();
                                txtDireccion.Text = reader["direccion"].ToString();
                                txtCalle.Text = reader["calle"].ToString();
                                txtPuerta.Text = reader["puerta"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos del deposito: " + ex.Message);
                }
            }
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
            string desc = txtDesc.Text;
            string direccion = txtDireccion.Text;
            string calle = txtCalle.Text;
            string puerta = txtPuerta.Text;


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE depositos SET descDeposito = @descDeposito, direccion = @direccion, calle = @calle, puerta = @puerta WHERE idDeposito = @depositoId;";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Asegúrate de encriptar la contraseña antes de guardarla
                        command.Parameters.AddWithValue("@descDeposito", desc);
                        command.Parameters.AddWithValue("@direccion", direccion);
                        command.Parameters.AddWithValue("@calle", calle);
                        command.Parameters.AddWithValue("@puerta", puerta);
                        command.Parameters.AddWithValue("@depositoId", depositoId);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Deposito agregado con éxito.");
                    this.Close(); // Cierra el formulario
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar deposito: " + ex.Message);
                }
            }
        }
    }
}
