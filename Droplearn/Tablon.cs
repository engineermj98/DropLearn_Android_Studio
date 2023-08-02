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
    public partial class Tablon : Form
    {
        int operacion = 0;
        int index;
        bool flag = false;
        InicioEstudiante inicio = new InicioEstudiante();
        public int cursos, idCursos;
        public string correo, nombreCurso, nombreProfesor, nombrePublicacion, FechaProxima, Comentarios,idTablon;
        public string teacher, student;
        Conexion conexion = new Conexion();
        ErrorProvider error = new ErrorProvider();
        public Tablon()
        {
            InitializeComponent();
        }

        private void Tablon_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 50, 50, 50);
            panel2.BackColor = Color.FromArgb(100, 50, 50, 50);
            panel3.BackColor = Color.FromArgb(100, 50, 50, 50);
            if (teacher == "Teacher")
            {
                btnAdd.Visible = true;
                btnEdit.Visible = true;
                btnDelete.Visible = true;
                lblBoard.Text = nombreCurso;
                lblAdd.Text = "Add a new publication";
                label6.Text = "Edit publication";
                label8.Text = "Delete publication";
                if (dgvTablon.SelectedRows.Count > 0)
                {
                    lblTablon.Visible = false;
                    idTablon = conexion.idTablon(nombrePublicacion, Convert.ToString(idCursos));
                }
                else
                {
                    lblTablon.Visible = true;
                    btnHomework.Enabled = false;
                }
            }
            else if (student == "Student")
            {
                lblAdd.Text = nombreCurso;
                label6.Text = conexion.nombreProfesor(nombreProfesor,nombreCurso);
                label8.Visible = false;
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
                if (dgvTablon.SelectedRows.Count > 0)
                {
                    lblTablon.Visible = false;
                    idTablon = conexion.idTablon(nombrePublicacion, Convert.ToString(idCursos));
                }
                else
                {
                    lblTablon.Visible = true;
                    btnHomework.Enabled = false;
                }
            }

            idCursos = conexion.idCursos(nombreCurso);
            dgvTablon.DataSource = conexion.cargarDatos("select Publicacion,FechaProxima,Comentarios from tablon where idCursos='" + idCursos + "'");
            dgvTablon.Columns[0].HeaderText = "Publications";
            dgvTablon.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvTablon.Columns[0].Width = 180;
            dgvTablon.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTablon.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvTablon.Columns[1].HeaderText = "Date";
            dgvTablon.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvTablon.Columns[1].Width = 120;
            dgvTablon.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTablon.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvTablon.Columns[2].HeaderText = "Comments";
            dgvTablon.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvTablon.Columns[0].ReadOnly = true;
            dgvTablon.Columns[1].ReadOnly = true;
            dgvTablon.Columns[2].ReadOnly = true;           
            dgvTablon.ClearSelection();

            panel5.Visible = false;
            

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
                lblTablon.Visible = false;
                InicioEstudiante volver4 = new InicioEstudiante();
                volver4.cur = cursos;
                volver4.correo2 = correo;
                volver4.student = student;
                volver4.Show();
            }
            this.Hide();
        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            operacion = 1;
            panel5.Visible = true;
            txtDate.Enabled = false;
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtPublication.Text = "";
            txtComment.Text = "";

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            flag = true;
            operacion = 2;
            if(dgvTablon.SelectedRows.Count > 0)
            {
                panel5.Visible = true;
                txtDate.Enabled = false;
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtPublication.Text = nombrePublicacion;
                txtComment.Text = Comentarios;
            }
            else
            {
                MessageBox.Show("First select a publication");
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(dgvTablon.SelectedRows.Count > 0)
            {
                DialogResult resp = MessageBox.Show("You are going to delete "+nombrePublicacion+".\nAre you sure?", "BOARD", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (resp == DialogResult.Yes)
                {
                    string query = "delete from tablon where Publicacion='"+nombrePublicacion+"'";
                    if (conexion.ejecucion(query))
                    {
                        MessageBox.Show(nombrePublicacion+" sucessfully deleted");
                    }
                    else
                    {
                        MessageBox.Show("Couldn't be deleted");
                    }
                }
            }
            else
            {
                MessageBox.Show("First select a publication");
            }
            
        }

        private void btnHomework_Click(object sender, EventArgs e)
        {
                if(dgvTablon.SelectedRows.Count > 0)
                {
                    lblBoard.Visible = false;
                    Trabajos trabajos = new Trabajos();
                    idTablon = conexion.idTablon(nombrePublicacion, Convert.ToString(idCursos));
                    trabajos.cursos = cursos;
                    trabajos.correo = correo;
                    trabajos.teacher = teacher;
                    trabajos.student = student;
                    trabajos.idCursos = idCursos;
                    trabajos.nombrePublicacion = nombrePublicacion;
                    trabajos.nombreProfesor = nombreProfesor;
                    trabajos.nombreCurso = nombreCurso;
                    trabajos.idTablon = idTablon;
                    trabajos.Show();
                }
                else
                {
                    lblBoard.Visible = true;
                    lblBoard.Text = "Please select a publication before clicking 'Homeworks'";
                }
                
                this.Hide();
            
        }

        private void dgvTablon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            index = dgvTablon.CurrentRow.Index;
            nombrePublicacion = dgvTablon.Rows[index].Cells[0].Value.ToString();
            FechaProxima = dgvTablon.Rows[index].Cells[1].Value.ToString();
            Comentarios = dgvTablon.Rows[index].Cells[2].Value.ToString();
            lblTablon.Visible = false;
            
            
            if(dgvTablon.SelectedRows.Count > 0)
            {
                btnHomework.Enabled = true;
                
            }
            else
            {
                btnHomework.Enabled = false;
            }
            

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean d = false, p = false, c = false;
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (txtDate.Text.Trim() == "")
            {
                error.SetError(txtDate, "Please write a date");
            }
            else
            {
                error.Clear();
                d = true;
            }
            if (txtPublication.Text.Trim() == "")
            {
                error.SetError(txtPublication, "Please write a publication");         
            }
            else
            {
                error.Clear();
                p = true;
            }
            if (txtComment.Text.Trim() == "")
            {
                error.SetError(txtComment, "Please write a comment");
            }
            else
            {
                error.Clear();
                c = true;
            }
            if (operacion == 1)//añadir
            {
                
                if(d == true && p == true && c == true)
                {
                    if (conexion.ejecucion("Insert into tablon(Publicacion,FechaProxima,Comentarios,idCursos) values ('" + txtPublication.Text + "','" + txtDate.Text + "','" + txtComment.Text + "','"+conexion.idCursos(nombreCurso)+"')"))
                    {
                        MessageBox.Show("Publication sucessfully created");
                        dgvTablon.DataSource = conexion.cargarDatos("select Publicacion,FechaProxima,Comentarios from tablon where idCursos='" + conexion.idCursos(nombreCurso) + "'");
                        dgvTablon.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("Couldn't be created");
                        dgvTablon.DataSource = conexion.cargarDatos("select Publicacion,FechaProxima,Comentarios from tablon where idCursos='" + conexion.idCursos(nombreCurso) + "'");
                        dgvTablon.ClearSelection();
                    }
                }
            }
            if(flag == true)//editar
            {
                if(dgvTablon.SelectedRows.Count > 0)
                {
                    if (d == true && p == true && c == true)
                    {
                        DialogResult resp = MessageBox.Show("You are going to modify "+nombrePublicacion+".\nAre you sure?", "BOARD", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (resp == DialogResult.Yes)
                        {
                            string query = "update tablon set Publicacion='" + txtPublication.Text + "',FechaProxima='" + txtDate.Text + "',Comentarios='" + txtComment.Text + "' where Publicacion='"+nombrePublicacion+"'";
                            if (conexion.ejecucion(query))
                            {
                                MessageBox.Show("Publication sucessfully modified");
                                dgvTablon.DataSource = conexion.cargarDatos("select Publicacion,FechaProxima,Comentarios from tablon where idCursos='" + conexion.idCursos(nombreCurso) + "'");
                                dgvTablon.ClearSelection();
                            }
                            else
                            {
                                MessageBox.Show("Couldn't be edited");
                                dgvTablon.DataSource = conexion.cargarDatos("select Publicacion,FechaProxima,Comentarios from tablon where idCursos='" + conexion.idCursos(nombreCurso) + "'");
                                dgvTablon.ClearSelection();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Date must be: yyyy-mm-dd");
                    }
                }
            }
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvTablon.DataSource = conexion.cargarDatos("select Publicacion,FechaProxima,Comentarios from tablon where idCursos='" + conexion.idCursos(nombreCurso) + "'");
            dgvTablon.ClearSelection();
            btnHomework.Enabled = false;
            operacion = 0;
            panel5.Visible = false;
            
        }


        

        private void panel1_Paint(object sender, PaintEventArgs e)
        {


            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(0, 139, 139), Color.FromArgb(220, 20, 60), LinearGradientMode.ForwardDiagonal);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.Vertical);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.BackwardDiagonal);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.ForwardDiagonal);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
        }

        
    }
}
