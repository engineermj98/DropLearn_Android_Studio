using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Droplearn
{
    public partial class Evaluaciones : Form
    {
        public int cursos, index;
        public string teacher, student;
        public string correo, validator,nombreCurso;
        public int idCursos;
        string idEvaluacion, progreso;
        Conexion conexion = new Conexion();

        public Evaluaciones()
        {
            InitializeComponent();
        }

        private void Evaluaciones_Load(object sender, EventArgs e)
        {
            
            

            if (teacher == "Teacher")
            {
                textBox4.Enabled = false;
                lblMessage.Visible = false;
                lblMessage2.Visible = false;
            }
            else if (student == "Student")
            {
                textBox1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;//seleccionar
                label4.Visible = false;
                label6.Visible = false;
                button3.Visible = false;
                button1.Visible = false;
                btnEdit.Visible = false;
                label9.Visible = false;
                textBox4.Visible = false;
                label5.Visible = false;
                textBox2.Visible = false;
                label7.Visible = false;
                textBox3.Visible = false;
                if (conexion.validarEvaluacion(correo))
                {
                    dgvScore.DataSource = conexion.cargarDatos("SELECT e.idEvaluaciones,c.Nombre,e.Actividades, e.Promedio, e.Progreso from evaluaciones  e INNER JOIN cursos c ON e.idCursos = c.idCursos  where Correo='" + correo + "'");
                    dgvScore.Columns[0].HeaderText = "ID";
                    dgvScore.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvScore.Columns[0].Width = 70;
                    dgvScore.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvScore.Columns[1].HeaderText = "COURSE";
                    dgvScore.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvScore.Columns[1].Width = 220;
                    dgvScore.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgvScore.Columns[2].HeaderText = "ACTIVITIES DELIVERED";
                    dgvScore.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvScore.Columns[2].Width = 100;
                    dgvScore.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvScore.Columns[3].HeaderText = "AVERAGE";
                    dgvScore.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvScore.Columns[4].HeaderText = "PROGRESS";
                    dgvScore.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvScore.ClearSelection();
                    dgvScore.ReadOnly = true;
                }
                else
                {
                    label3.Text = "First join into a course to create an evaluation profile.";
                }

            }
        }

        private void volver1_Click(object sender, EventArgs e)
        {
            if (teacher == "Teacher")
            {
                InicioProfesor volverProfe2 = new InicioProfesor();
                volverProfe2.cur = cursos;
                volverProfe2.correo2 = correo;
                volverProfe2.teacher = teacher;
                volverProfe2.Show();
            }
            else if (student == "Student")
            {
                InicioEstudiante volver3 = new InicioEstudiante();
                volver3.cur = cursos;
                volver3.correo2 = correo;
                volver3.student = student;
                volver3.Show();
            }
            this.Hide();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.Vertical);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.Vertical);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
        }

    

      

        private void dgvScore_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox4.Enabled = false;
            int index = dgvScore.CurrentRow.Index;
            idEvaluacion = dgvScore.Rows[index].Cells[0].Value.ToString();
            textBox4.Text = idEvaluacion;
            progreso = dgvScore.Rows[index].Cells[2].Value.ToString();
            textBox3.Text = progreso;
        }

       

        private void button1_Click(object sender, EventArgs e) // buscador
        {
            // textbox1 toma lo que ingresas en el buscador que en este caso es un correo, no se pone correo por que te estara tomando el del maestro y no de un estudiante
            if (conexion.validarEvaluacion(textBox1.Text))
            {
                dgvScore.DataSource = conexion.cargarDatos("SELECT e.idEvaluaciones,c.Nombre,e.Actividades, e.Promedio, e.Progreso from evaluaciones  e INNER JOIN cursos c ON e.idCursos = c.idCursos  where Correo='" + textBox1.Text + "'");
                dgvScore.Columns[0].HeaderText = "ID";
                dgvScore.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvScore.Columns[1].HeaderText = "COURSE";
                dgvScore.Columns[1].Width = 160; //id
                dgvScore.Columns[1].Width = 190; // nombre curso
                dgvScore.Columns[2].HeaderText = "ACTIVITIES";
                dgvScore.Columns[2].Width = 140; // actividades
                dgvScore.Columns[3].HeaderText = "AVERAGE";
                dgvScore.Columns[3].Width = 120; // promedio
                dgvScore.Columns[4].HeaderText = "PROGRESS";
                dgvScore.Columns[4].Width = 90; // progreso
                dgvScore.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvScore.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvScore.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvScore.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvScore.ReadOnly = true;
                
            }
            else
            {
                MessageBox.Show("Must be an existing student email");
            }
            label3.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) // guardar cambios 
        {

            if (textBox2.Text.Trim()=="" || textBox3.Text.Trim() == "")
            {
                MessageBox.Show("no empty fields allowed");
            }
            else
            {
                DialogResult resp = MessageBox.Show("You're going to Modify your Data. \n Are you sure?", "COURSE", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (resp == DialogResult.Yes)
                {

                    dgvScore.DataSource = conexion.ejecucion("update evaluaciones set Promedio ='" + textBox2.Text + "', Progreso ='" + textBox3.Text + "' Where idEvaluaciones ='" + textBox4.Text + "' ");
                    dgvScore.ClearSelection();
                }
                MessageBox.Show("succesfully modified");
            }
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            label3.Visible = true;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;

        }

        private void btnEdit_Click(object sender, EventArgs e)// habilitas lo cambios
        {
            idCursos = conexion.idCursos(nombreCurso);
            label4.Visible = true;
            button3.Visible = true;
            textBox2.Enabled = true;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
        }

     

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.Vertical);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
        }

        
    }
}
