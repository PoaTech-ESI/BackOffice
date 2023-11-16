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
    public partial class AddPaquetesForm : Form
    {
        private TextBox txtDesc, txtPeso, txtDireccion;
        private ComboBox comboBoxLote, comboBoxPropietario, comboBoxEstado, comboBoxDeposito;
        private Button btnSave, btnCancel;
        private string connectionString = "server=44.201.241.190;database=proyecto2023;uid=root;pwd=89Vg3zaKAJJjKvCFDPmARPmjyMfV27Bk;";


        public AddPaquetesForm()
        {
            InitializeComponent();
            InitializeFormControls();
        }

        private void InitializeFormControls()
        {
            int yPos = 10;

            // Método para crear un par de Label y TextBox para cada campo
            txtDesc = CreateTextbox("Descripcion", yPos);
            txtPeso = CreateTextbox("Peso", yPos += 30);
            txtDireccion = CreateTextbox("Direccion Destino", yPos += 30);

            // ComboBox para Lote
            CreateLabel("Lote Asignado", yPos += 30);
            comboBoxLote = new ComboBox { Left = 110, Top = yPos, Width = 170 };
            this.Controls.Add(comboBoxLote);

            // ComboBox para Propietario
            CreateLabel("Propietario", yPos += 30);
            comboBoxPropietario = new ComboBox { Left = 110, Top = yPos, Width = 170 };
            this.Controls.Add(comboBoxPropietario);

            // ComboBox para Estado
            CreateLabel("Estado", yPos += 30);
            comboBoxEstado = new ComboBox { Left = 110, Top = yPos, Width = 170 };
            this.Controls.Add(comboBoxEstado);

            // ComboBox para Deposito
            CreateLabel("Deposito", yPos += 30);
            comboBoxDeposito = new ComboBox { Left = 110, Top = yPos, Width = 170 };
            this.Controls.Add(comboBoxDeposito);

            // Botones Guardar y Cancelar
            btnSave = new Button { Text = "Guardar", Left = 50, Top = yPos += 30, Width = 100 };
            btnSave.Click += new EventHandler(btnSave_Click);
            this.Controls.Add(btnSave);

            btnCancel = new Button { Text = "Cancelar", Left = 160, Top = yPos, Width = 100 };
            btnCancel.Click += (sender, e) => { this.Close(); };
            this.Controls.Add(btnCancel);

            // Cargar roles en el ComboBox (Este método debe ser implementado)
            CargarLote();
            CargarPropietario();
            CargarEstado();
            CargarDeposito();

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
            string desc = txtDesc.Text;
            string peso = txtPeso.Text;
            string direccion = txtDireccion.Text;
            int loteId = Convert.ToInt32(comboBoxLote.SelectedValue);
            int propietarioId = Convert.ToInt32(comboBoxPropietario.SelectedValue);
            int estadoId = Convert.ToInt32(comboBoxEstado.SelectedValue);
            int depositoId = Convert.ToInt32(comboBoxDeposito.SelectedValue);


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO paquetes(idLote, descripcion, peso, direccionDestino, propietario, estado, idDeposito) VALUES (@idLote, @descripcion, @peso, @direccionDestino, @propietario, @estado, @idDeposito)\r\n";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Asegúrate de encriptar la contraseña antes de guardarla
                        command.Parameters.AddWithValue("@idLote", loteId);
                        command.Parameters.AddWithValue("@descripcion", desc);
                        command.Parameters.AddWithValue("@peso", peso);
                        command.Parameters.AddWithValue("@direccionDestino", direccion);
                        command.Parameters.AddWithValue("@propietario", propietarioId);
                        command.Parameters.AddWithValue("@estado", estadoId);
                        command.Parameters.AddWithValue("@idDeposito", depositoId);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Paquete agregado con éxito.");
                    this.Close(); // Cierra el formulario
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar paquete: " + ex.Message);
                }
            }
        }


        private void CargarLote()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT idLote, descLote FROM lotes";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBoxLote.DisplayMember = "descLote";
                    comboBoxLote.ValueMember = "idLote";
                    comboBoxLote.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar lotes: " + ex.Message);
                }
            }
        }

        private void CargarPropietario()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT idUser, username FROM usuarios WHERE idRol = 1;";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBoxPropietario.DisplayMember = "username";
                    comboBoxPropietario.ValueMember = "idUser";
                    comboBoxPropietario.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar propietarios: " + ex.Message);
                }
            }
        }
        private void CargarEstado()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT idStatus, descStatus FROM statusEnvio;";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBoxEstado.DisplayMember = "descStatus";
                    comboBoxEstado.ValueMember = "idStatus";
                    comboBoxEstado.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar estado: " + ex.Message);
                }
            }
        }
        private void CargarDeposito()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT idDeposito, descDeposito FROM depositos;";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBoxDeposito.DisplayMember = "descDeposito";
                    comboBoxDeposito.ValueMember = "idDeposito";
                    comboBoxDeposito.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar deposito: " + ex.Message);
                }
            }
        }
    }
}
