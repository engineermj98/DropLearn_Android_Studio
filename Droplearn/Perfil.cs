using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Droplearn
{
    public partial class Perfil : Form
    {
        public int cursos;
        public string correo, perfil, vacio;
        public string teacher,student;

        Conexion conexion = new Conexion();
        public Perfil()
        {
            InitializeComponent();
        }

        private void Perfil_Load(object sender, EventArgs e)
        {

            panel1.BackColor = Color.FromArgb(100, 50, 50, 50);
            // MessageBox.Show("correo:" + correo);
            // MessageBox.Show("maestro o estu: " + teacher);
            textBox1.Text = conexion.Cursos(correo);
            textBox2.Text = correo;
            textBox3.Text = conexion.Escolaridad(correo);
            textBox4.Text = conexion.NiveldeEstudios(correo);
            textBox5.Text = conexion.Edad(correo);

            perfil = conexion.idPerfil(correo);

            vacio = conexion.Imagen(correo);

            if (vacio != "" || vacio != null || vacio !="NULL")
            {
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                byte[] abyte = ms.ToArray();

                MySqlConnection conexionBD = conexion.conectando();
                conexionBD.Open();


                string query2 = "select imagen from perfil where idPerfil='" + perfil + "'";
                try
                {
                    MySqlCommand comando = new MySqlCommand(query2, conexionBD);
                    MySqlDataReader reader = comando.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!Convert.IsDBNull(reader["imagen"]))
                        {
                            MemoryStream ms2 = new MemoryStream((byte[])reader["imagen"]);
                            Bitmap bm = new Bitmap(ms2);
                            pictureBox1.Image = bm;
                        }
                       


                    }
                    else
                    {
                        MessageBox.Show("it doesn't exits register ");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);
                }

            }




            button3.Visible = false;
            label11.Visible = false;

        }

        private void volver1_Click(object sender, EventArgs e)
        {
            
            if (teacher == "Teacher")
            {
                InicioProfesor volverProfe3 = new InicioProfesor();
                volverProfe3.cur = cursos;
                volverProfe3.correo2 = correo;
                volverProfe3.teacher = teacher;
                volverProfe3.Show();
            }
            else if(student == "Student")
            {
                InicioEstudiante volver = new InicioEstudiante();
                volver.cur = cursos;
                volver.correo2 = correo;
                volver.student = student;
                volver.Show();
            }

            this.Hide();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.Horizontal);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.Horizontal);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
        }

     

       

        private void button1_Click(object sender, EventArgs e) // borrar 
        {
            Login ver = new Login();

            DialogResult resp = MessageBox.Show("You're going to Delete your account. \n Are you sure?", "COURSE", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (resp == DialogResult.Yes)
            {
                conexion.ejecucion("delete from perfil where idPerfil= '" + perfil + "'");
                conexion.ejecucion("delete from registro where Correo= '" + correo + "'");
                ver.Show();
                this.Hide();

            }
      

        }

        private void button3_Click(object sender, EventArgs e)//update
        {
            

            if (textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" || textBox4.Text.Trim() == "" || textBox5.Text.Trim() == "")
            {
                MessageBox.Show("no empty fields allowed");
            }
            else
            {
                DialogResult resp = MessageBox.Show("You're going to Modify your Data. \n Are you sure?", "COURSE", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (resp == DialogResult.Yes)
                {
                    conexion.ejecucion("Update perfil SET  Correo = '" + textBox2.Text + "',Escolaridad = '" + textBox3.Text + "',NiveldeEstudios = '" + textBox4.Text + "', edad = '" + textBox5.Text + "' where idPerfil ='" + perfil + "'");
                    conexion.ejecucion("Update registro set Correo = '" + textBox2.Text + "' where Correo='"+correo+"'");
                    MessageBox.Show("succesfully modified");
                }
                
            }
            

            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;


        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)// boton cambiar imagen
        {


            OpenFileDialog seleccionar = new OpenFileDialog();
            seleccionar.Filter = "Imagenes | *.jpg; *.png";
            seleccionar.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            seleccionar.Title = "Seleccionar imagen";

            if (seleccionar.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(seleccionar.FileName);

            }


            DialogResult resp = MessageBox.Show("You're going to Modify your Image. \n Are you sure?", "COURSE", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (resp == DialogResult.Yes)
            {

                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                byte[] abyte = ms.ToArray();

                MySqlConnection conexionBD = conexion.conectando();
                conexionBD.Open();

                try
                {
                    MySqlCommand comando = new MySqlCommand("update perfil SET imagen = @imagen where idPerfil='" + perfil + "'", conexionBD);
                    comando.Parameters.AddWithValue("imagen", abyte);
                    comando.ExecuteNonQuery();
                    pictureBox1.Image = null;

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al guardar imagen" + ex.Message);
                }

                string query2 = "select imagen from perfil where idPerfil='" + perfil + "'";
                try
                {
                    MySqlCommand comando = new MySqlCommand(query2, conexionBD);
                    MySqlDataReader reader = comando.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!Convert.IsDBNull(reader["imagen"]))
                        {
                            MemoryStream ms2 = new MemoryStream((byte[])reader["imagen"]);
                            Bitmap bm = new Bitmap(ms2);
                            pictureBox1.Image = bm;
                        }


                    }
                    else
                    {
                        MessageBox.Show("it doesn't exits register ");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);
                }

            }
            else if (resp == DialogResult.No)
            {
                string query2 = "select imagen from perfil where idPerfil='" + perfil + "'";
                MySqlConnection conexionBD = conexion.conectando();
                conexionBD.Open();

                try
                {
                    MySqlCommand comando = new MySqlCommand(query2, conexionBD);
                    MySqlDataReader reader = comando.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!Convert.IsDBNull(reader["imagen"]))
                        {
                            MemoryStream ms2 = new MemoryStream((byte[])reader["imagen"]);
                            Bitmap bm = new Bitmap(ms2);
                            pictureBox1.Image = bm;
                        }

                    }
                    else
                    {
                        MessageBox.Show("it doesn't exits register ");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al buscar" + ex.Message);
                }

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e) // click para editar
        {
           
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            button3.Visible = true;
            label11.Visible = true;

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
           

        }

        private void btnRefresh_Click(object sender, EventArgs e) // refrescar la imagen
        {

            textBox1.Text = conexion.Cursos(correo);
            textBox2.Text = correo;
            textBox3.Text = conexion.Escolaridad(correo);
            textBox4.Text = conexion.NiveldeEstudios(correo);
            textBox5.Text = conexion.Edad(correo);

            //textBox1.Text= conexion.cargarDatos("select Nombre,Clave,Profesor from cursos;");

            string query = "select imagen from perfil where idPerfil='" + perfil + "'";


            MySqlConnection conexionBD = conexion.conectando();
            conexionBD.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(query, conexionBD);
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    if (!Convert.IsDBNull(reader["imagen"]))
                    {
                        MemoryStream ms2 = new MemoryStream((byte[])reader["imagen"]);
                        Bitmap bm = new Bitmap(ms2);
                        pictureBox1.Image = bm;
                    }
                  
                    

                }
                else
                {
                    MessageBox.Show("it doesn't exits register ");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al buscar" + ex.Message);
            }


        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.Horizontal);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
        }

        
    }
}
