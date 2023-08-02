using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Droplearn
{
    public partial class Trabajos : Form
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        Conexion conexion = new Conexion();
        bool flag = false;
        bool flag2 = false;
        public string teacher, student, nombrePublicacion;
        public int cursos,idCursos;
        public string correo,nombreProfesor, nombreCurso,idTablon;
        string filerealname;
        byte[] file,document;
        string fechaEntrega,nombreArchivo;
        string idTrabajo;
        public string idCursosTrabajos;
        bool flagDelete = false;
        public int actaux, progaux,act,prog,totalActividades,regla3;//los contadores que van a aumentar actividades yp rogreso de evaluaciones y los que almacenaran el valor real de la bd.
        public Trabajos()
        {
            InitializeComponent();
        }

        private void Trabajos_Load(object sender, EventArgs e) 
        {
            
            panel1.BackColor = Color.FromArgb(60, 50, 50, 50);
            panel2.BackColor = Color.FromArgb(80, 50, 50, 50);
            panel3.BackColor = Color.FromArgb(70, 50, 50, 50);
            //conexion.llenarCombo(cbCourse);
            //MessageBox.Show("cantidad de actividades de tablon:" + Convert.ToInt32(conexion.tablonActividades(Convert.ToString(idCursos))));
            //MessageBox.Show("cantidad de actividades de trabajos:" + Convert.ToInt32(conexion.trabajosActividades(correo, idTablon)));
            panel4.Visible = false;
            lblBoard.Text = nombrePublicacion;
            btnOpenFile.Visible = false;
            btnDelFile.Visible = false;
            btnCheck.Visible = false;
            lblDeliver.Visible = false;
            if (teacher == "Teacher")
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
                lblAdd.Visible = false;
                label6.Visible = false;
                label8.Visible = false;
                totalActividades = Convert.ToInt32(conexion.tablonActividades(Convert.ToString(idCursos)));
                dgvTrabajos.DataSource = conexion.cargarDatos("select idTrabajo,Nombre,Descripcion,FechaEntrega,Correo from trabajos where idTablon='" + idTablon + "'");
                dgvTrabajos.Columns[0].Visible = false;
                dgvTrabajos.Columns[1].HeaderText = "TITLE";
                dgvTrabajos.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvTrabajos.Columns[1].Width = 170;
                dgvTrabajos.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTrabajos.Columns[2].HeaderText = "DESCRIPTION";
                dgvTrabajos.Columns[3].HeaderText = "DELIVERED";
                dgvTrabajos.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvTrabajos.Columns[3].Width = 120;
                dgvTrabajos.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTrabajos.Columns[4].HeaderText = "STUDENT EMAIL";
                dgvTrabajos.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTrabajos.ReadOnly = true;
            }
            else
            if (student == "Student")
            {
                lblAdd.Text = "Add homework";
                label6.Text = "Edit homework";
                label8.Text = "Delete homework";
                idCursosTrabajos = conexion.idCursosTrabajos(correo,idTablon);
                act = Convert.ToInt32(conexion.trabajosActividades(correo, idCursosTrabajos));
                //MessageBox.Show("idcursos=" + idCursosTrabajos);
                //MessageBox.Show("actaux=" + actaux);
                //dgvTrabajos.DataSource = conexion.cargarDatos("select idCursos from tablon inner join trabajos on tablon.idTablon = trabajos.idTablon where trabajos.Correo='"+correo+"' and trabajos.idTablon='"+idTablon+"'");
                //dgvTrabajos.DataSource = conexion.cargarDatos("select count(*) from tablon inner join trabajos on tablon.idTablon = trabajos.idTablon where tablon.idCursos='" + idCursosTrabajos + "' and trabajos.Correo='" + correo + "'");
                
                dgvTrabajos.DataSource = conexion.cargarDatos("select idTrabajo,Nombre,Descripcion,FechaEntrega,Correo from trabajos where idTablon='" + idTablon + "' and Correo='" + correo + "'");                           
                dgvTrabajos.Columns[0].Visible = false;
                dgvTrabajos.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvTrabajos.Columns[1].HeaderText = "TITLE";
                dgvTrabajos.Columns[1].Width = 220;
                dgvTrabajos.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvTrabajos.Columns[2].Width = 250;
                dgvTrabajos.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTrabajos.Columns[3].Width = 290;
                dgvTrabajos.Columns[4].Visible = false;               
                dgvTrabajos.ReadOnly = true;
                
            }
            dgvTrabajos.ClearSelection();
        }

       

        private void btnFile_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = openFileDialog.FileName;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {

        }

       

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnCheck.Enabled = true;
            if (txtFilename.Text.Trim().Equals(""))
            {
                MessageBox.Show("Filename field required");
                return;
            }
            try
            {      
                file = null;
                Stream myStream = openFileDialog.OpenFile();
                using (MemoryStream ms = new MemoryStream())
                {
                    myStream.CopyTo(ms);
                    file = ms.ToArray();
                }

                filerealname = openFileDialog.SafeFileName;
                document = file;
                flag2 = true;
            }
            catch(Exception ex)
            {
                flag2 = false;
                MessageBox.Show("Please first open a file ");
                lblDeliver.Visible = false;
                btnCheck.Visible = false;
            }   
            if (flag == true && flag2 == true)//si es añadir y a parte ya se abrio un documento, entonces
            {
                btnCheck.Visible = true;
                lblDeliver.Visible = true;
                fechaEntrega = DateTime.Now.ToString("yyyy-MM-dd");
                totalActividades = Convert.ToInt32(conexion.tablonActividades(Convert.ToString(idCursos)));
                if (conexion.validarTrabajos(idTablon,correo))//Si existe un trabajo en el tablon de una publicacion entonces
                {
                    txtFilename.Text = "";
                    txtFile.Text = "";
                    lblDeliver.Visible = false;
                    btnCheck.Visible = false;
                    MessageBox.Show("Sorry but task is already delivered");                   
                    panel4.Visible = false;
                    dgvTrabajos.DataSource = conexion.cargarDatos("select idTrabajo,Nombre,Descripcion,FechaEntrega,Correo from trabajos where idTablon='" + idTablon + "' and Correo='" + correo + "'");
                    dgvTrabajos.Columns[0].Visible = false;
                    dgvTrabajos.ClearSelection();
                }
                else
                {
                    string query = "Insert into trabajos (Nombre,Descripcion,FechaEntrega,Documento,Correo,idTablon) values ('" + txtFilename.Text + "','" + filerealname + "','" + fechaEntrega + "','" + document + "','" + correo + "'," + idTablon + ")";

                    if (conexion.ejecucion(query))
                    {
                        MessageBox.Show("Please now mark your homework as checked to deliver it");
                        btnCheck.Focus();
                    }    
                } 
            }   
            else if(flag == false)
            {
                
                DialogResult resp = MessageBox.Show("You're going to modify " + nombreArchivo + "\n Are you sure?", "COURSE", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (resp == DialogResult.Yes)
                {
                    btnCheck.Visible = false;
                    lblDeliver.Visible = false;
                    string query = "update trabajos set Nombre='" + txtFilename.Text + "',Descripcion='" + filerealname + "', Documento='" + document + "' where idTrabajo='" + idTrabajo + "'";
                    if (conexion.ejecucion(query))
                    {
                        MessageBox.Show("Succesfully edited");
                    }
                    dgvTrabajos.DataSource = conexion.cargarDatos("select idTrabajo,Nombre,Descripcion,FechaEntrega,Correo from trabajos where idTablon='" + idTablon + "'");
                    dgvTrabajos.Columns[0].Visible = false;
                    dgvTrabajos.ClearSelection();
                }
                    
            }
            btnSaveFile.Enabled = false;
        }

        private void dgvTrabajos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvTrabajos.CurrentRow.Index;
            idTrabajo = dgvTrabajos.Rows[index].Cells[0].Value.ToString();
            nombreArchivo = dgvTrabajos.Rows[index].Cells[1].Value.ToString();
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            flag = false;           
            btnCheck.Visible = false;
            lblDeliver.Visible = false;
            btnSaveFile.Enabled = true;
            flagDelete = false;
            if(dgvTrabajos.SelectedRows.Count > 0)
            {
                panel4.Visible = true;
                txtFilename.Text = nombreArchivo;
                txtFile.Text = conexion.trabajosDescripcion(nombreArchivo);
            }
            else
            {
                MessageBox.Show("Please first select a task.");
            }
            dgvTrabajos.DataSource = conexion.cargarDatos("select idTrabajo,Nombre,Descripcion,FechaEntrega,Correo from trabajos where idTablon='" + idTablon + "' and Correo='" + correo + "'");
            dgvTrabajos.Columns[0].Visible = false;
            dgvTrabajos.ClearSelection();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool flag = false;
            lblDeliver.Visible = false;
            btnCheck.Visible = false;
            idCursosTrabajos = conexion.idCursosTrabajos(correo, idTablon);
            act = Convert.ToInt32(conexion.trabajosActividades(correo, idCursosTrabajos));
            //MessageBox.Show("idCtr" + idCursosTrabajos);
                DialogResult resp = MessageBox.Show("You're going to delete " + nombreArchivo + "\n Are you sure?", "COURSE", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (resp == DialogResult.Yes)
                {       
                    string query = "delete from trabajos where idTrabajo='" + idTrabajo + "'";
                    
                if (conexion.ejecucion(query))
                {
                    MessageBox.Show("Succesfully deleted");
                }
                act = Convert.ToInt32(conexion.trabajosActividades(correo, idCursosTrabajos));
                //DEspues de ejecutarse el query y se elimina una tarea, entonces actividades va a ser igual a si mismo pero -1
                //MessageBox.Show("idTablon" + idTablon);
                //MessageBox.Show("correo" + correo);
                //MessageBox.Show("idCtr" + idCursos);
                //MessageBox.Show("act" + act);
                string queryUpdate = "update evaluaciones set Actividades='" + act + "' where Correo='" + correo + "' and idCursos='" + idCursosTrabajos + "'";
                conexion.ejecucion(queryUpdate);
                totalActividades = Convert.ToInt32(conexion.tablonActividades(Convert.ToString(idCursos)));
                prog = (Convert.ToInt32(conexion.trabajosActividades(correo, idCursosTrabajos)) * 100) / totalActividades;
                string queryUpdate2 = "update evaluaciones set Progreso='" + prog + "' where Correo='" + correo + "' and idCursos='" + idCursosTrabajos + "'";
                conexion.ejecucion(queryUpdate2);
            }      
            dgvTrabajos.DataSource = conexion.cargarDatos("select idTrabajo,Nombre,Descripcion,FechaEntrega,Correo from trabajos where idTablon='" + idTablon + "' and Correo='" + correo + "'");
            dgvTrabajos.Columns[0].Visible = false;
            dgvTrabajos.ClearSelection();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dgvTrabajos.DataSource = conexion.cargarDatos("select idTrabajo,Nombre,Descripcion,FechaEntrega,Correo from trabajos where idTablon='" + idTablon + "' and Correo='" + correo + "'");
            dgvTrabajos.Columns[0].Visible = false;
            dgvTrabajos.ClearSelection();
            flag = true;
            btnSaveFile.Enabled = true;
            flagDelete = false;
            panel4.Visible = true;
            txtFilename.Text = "";
            txtFile.Text = "";
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {

            idCursosTrabajos = conexion.idCursosTrabajos(correo, idTablon);
            act = Convert.ToInt32(conexion.trabajosActividades(correo, idCursosTrabajos));
            prog = conexion.progreso(correo, Convert.ToString(idCursos));
            totalActividades = Convert.ToInt32(conexion.tablonActividades(Convert.ToString(idCursos)));
            prog = (Convert.ToInt32(Convert.ToInt32(conexion.trabajosActividades(correo, idCursosTrabajos))) * 100) / totalActividades;
            string queryUpdate = "update evaluaciones set Actividades='" + act + "', Progreso='"+prog+"' where Correo='"+correo+"' and idCursos='"+idCursosTrabajos+"'";
            conexion.ejecucion(queryUpdate);
            btnCheck.Enabled = false;
            flagDelete = false;
            dgvTrabajos.DataSource = conexion.cargarDatos("select idTrabajo,Nombre,Descripcion,FechaEntrega,Correo from trabajos where idTablon='" + idTablon + "' and Correo='" + correo + "'");
            dgvTrabajos.Columns[0].Visible = false;
            dgvTrabajos.ClearSelection();
            panel4.Visible = false;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (teacher == "Teacher")
            {
                Tablon tablon = new Tablon();
                tablon.cursos = cursos;
                tablon.correo = correo;
                tablon.teacher = teacher;
                tablon.idCursos = idCursos;
                tablon.nombrePublicacion = nombrePublicacion;
                tablon.nombreProfesor = nombreProfesor;
                tablon.nombreCurso = nombreCurso;
                tablon.idTablon = idTablon;
                tablon.Show();
            }
            else if (student == "Student")
            {
                Tablon tablon = new Tablon();
                tablon.cursos = cursos;
                tablon.correo = correo;
                tablon.student = student;
                tablon.idCursos = idCursos;
                tablon.nombrePublicacion = nombrePublicacion;
                tablon.nombreProfesor = nombreProfesor;
                tablon.nombreCurso = nombreCurso;
                tablon.idTablon = idTablon;
                tablon.Show();
            }
            this.Hide();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            flagDelete = false;
            if(teacher == "Teacher")
            {
                dgvTrabajos.DataSource = conexion.cargarDatos("select idTrabajo,Nombre,Descripcion,FechaEntrega,Correo from trabajos where idTablon='" + idTablon + "'");
                dgvTrabajos.Columns[0].Visible = false;
                dgvTrabajos.ClearSelection();
            }
            else if(student == "Student")
            {
                dgvTrabajos.DataSource = conexion.cargarDatos("select idTrabajo,Nombre,Descripcion,FechaEntrega,Correo from trabajos where idTablon='" + idTablon + "' and Correo='" + correo + "'");
                dgvTrabajos.Columns[0].Visible = false;
                dgvTrabajos.ClearSelection();
            }
            
        }

        

        private void volver1_Click(object sender, EventArgs e)
        {
            if (teacher == "Teacher")
            {
                InicioProfesor volverProfe = new InicioProfesor();
                volverProfe.cur = cursos;
                volverProfe.correo2 = correo;
                volverProfe.teacher = teacher;
                volverProfe.Show();
            }
            else if (student == "Student")
            {
                InicioEstudiante volver4 = new InicioEstudiante();
                volver4.cur = cursos;
                volver4.correo2 = correo;
                volver4.student = student;
                volver4.Show();
            }
            this.Hide();
        }
        private void panel4_Paint(object sender, PaintEventArgs e)
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
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.ForwardDiagonal);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
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
