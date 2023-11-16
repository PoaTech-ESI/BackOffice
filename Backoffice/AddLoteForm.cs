﻿using MySql.Data.MySqlClient;
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
    public partial class AddLoteForm : Form
    {
        // Declaraciones de los controles
        private TextBox txtDesc, txtBarrio;
        private Button btnSave, btnCancel;
        private string connectionString = "server=44.201.241.190;database=proyecto2023;uid=root;pwd=89Vg3zaKAJJjKvCFDPmARPmjyMfV27Bk;";


        public AddLoteForm()
        {
            InitializeComponent();
            InitializeFormControls();
        }

        private void InitializeFormControls()
        {
            int yPos = 10;

            // Método para crear un par de Label y TextBox para cada campo
            txtDesc = CreateTextbox("Desc Lote", yPos);
            txtBarrio = CreateTextbox("Barrio", yPos += 30);


            // Botones Guardar y Cancelar
            btnSave = new Button { Text = "Guardar", Left = 50, Top = yPos += 30, Width = 100 };
            btnSave.Click += new EventHandler(btnSave_Click);
            this.Controls.Add(btnSave);

            btnCancel = new Button { Text = "Cancelar", Left = 160, Top = yPos, Width = 100 };
            btnCancel.Click += (sender, e) => { this.Close(); };
            this.Controls.Add(btnCancel);



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
            string barrio = txtBarrio.Text;


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO lotes (descLote, barrioDestino) VALUES (@username, @password)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Asegúrate de encriptar la contraseña antes de guardarla
                        command.Parameters.AddWithValue("@username", desc);
                        command.Parameters.AddWithValue("@password", barrio);
          
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Lote agregado con éxito.");
                    this.Close(); // Cierra el formulario
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar lote: " + ex.Message);
                }
            }
        }

    }
}
