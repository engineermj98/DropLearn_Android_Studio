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
using System.Data.SqlClient;
using MySql.Data.MySqlClient;


namespace Droplearn
{
    public partial class Login : Form
    {
        public string correo;
        Conexion conexion = new Conexion();
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 50, 50, 50);
            conexion.idReg();
        }

        private void Login_Paint(object sender, PaintEventArgs e)
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
            
            Boolean t1 = false, t2 = false;
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-z]\\.)+[a-zA-Z]{2,9})$";

            if (textBox1.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox1, "write email");


            }
            else
            {
                errorProvider1.Clear();
                if (Regex.IsMatch(textBox1.Text, pattern))
                {
                    t1 = true;
             
                }
                else
                {
                    errorProvider1.SetError(textBox1, "invalid email");

                }
            }
            if (textBox2.Text.Trim() == "")
            {
                errorProvider1.SetError(textBox2, "write password");

            }
            else if (textBox2.Text.Length > 8)
            {
                errorProvider1.SetError(textBox2, "password should be of 8 characters");
            }
            else
            {
                t2 = true;

            }
            InicioEstudiante inicio = new InicioEstudiante();
            InicioProfesor inicioP = new InicioProfesor();
            conexion.correo = textBox1.Text;
            conexion.correo3 = textBox1.Text;
            inicio.correo2 = textBox1.Text;
            inicioP.correo2 = textBox1.Text;
            
            if (t1 == true && t2 == true)
            {
                conexion.logear(textBox1.Text, textBox2.Text);
                if(conexion.flag == 1) //Si en conexion logear reconoce el usuario y pasa a la siguiente pantalla, que se esconda la vista Login, pero si el usuario es incorrecto, que se siga viendo.
                {
                    this.Visible = false;
                }
            }
        }

        private void registro_Click(object sender, EventArgs e)
        {
            Registro inicio = new Registro();
            inicio.Show();

            this.Hide();
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                if (textBox2.PasswordChar == '*')
                {
                    textBox2.PasswordChar = '\0';
                }
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }


    }
}
