using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Droplearn
{
    public partial class Registro : Form
    {
        Conexion conexion = new Conexion();
        public Registro()
        {
            InitializeComponent();
        }

        private void Registro_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 50, 50, 50);
        }

        private void Registro_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.ForwardDiagonal);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
            
        }

        private void logeo_Click(object sender, EventArgs e)
        {
            Login ventana = new Login();
            ventana.Show();

            this.Hide();
        }

        private void registrarse_Click(object sender, EventArgs e)
        {
            String tipo = "";
            Boolean t1 =false, t2 = false, t3 = false;
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-z]\\.)+[a-zA-Z]{2,9})$";

            if (textBox1.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox1, "write username");


            }
            else
            {
                errorProvider1.Clear();
                t1 = true;
            } 
            if (textBox2.Text.Trim() == "" ) 
            {
                
                errorProvider1.SetError(textBox2, "write email");


            }
            else
            {
                errorProvider1.Clear();

                
                if (Regex.IsMatch(textBox2.Text, pattern))
                {
                    t2 = true;
                }
                else
                {
                    errorProvider1.SetError(textBox2, "invalid email");


                }

            }

            if (textBox3.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox3, "write password");

            }
            else if (textBox3.Text.Length > 8)
            {
                errorProvider1.SetError(textBox3, "password should be of 8 characters");



            }
            else if(textBox3.Text.Length < 8)
            {
                t3 = true;
            }

            if (radioButton1.Checked)
            {
                tipo += radioButton1.Text;

            }
            else
            {
                tipo += radioButton2.Text;
            }
            

            // insertar en la base de datos
            if (t1 ==true && t2 == true && t3 == true)
            {
                conexion.ejecucion("Insert Into registro (Nombre,Correo,TipodeUsuario,Contraseña) VALUES ('"+textBox1.Text+"','"+ textBox2.Text + "'" +
                    ",'"+tipo+"','"+ textBox3.Text + "')");
                
                if(conexion.ejecucion("Insert Into perfil (Cursos,Correo, idRegistro) VALUES (0,'" + textBox2.Text + "','" + conexion.idReg() + "')"))
                {
                    MessageBox.Show("User successfully registered");
                    Login ventana = new Login();
                    //MessageBox.Show(Convert.ToString(conexion.idReg()));
                    ventana.Show();
                    this.Hide();
                }

            }
            else
            {
                MessageBox.Show("ERROR");
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                if (textBox3.PasswordChar == '*')
                {
                    textBox3.PasswordChar = '\0';
                }
            }
            else
            {
                textBox3.PasswordChar = '*';
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
