using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Droplearn
{
    public partial class InicioProfesor : Form
    {
        public int operacion = 0;//El que definira si es editar, crear o eliminar
        public int index;
        bool flagRefresh = false;
        bool flag = false;
        bool flag2 = false; //PAra poder editar y añadir por separado en btn Ok click

        public int cur,cuentaCursos;
        string nombreProfesor;
        string nombre, clave, titleCurso;
        public string teacher;
        public string correo2, nombreCurso,publicacion;
        Random rnd = new Random();
        Conexion conexion = new Conexion();
        ErrorProvider errorProvider = new ErrorProvider();
        public InicioProfesor()
        {
            InitializeComponent();
        }
        private void InicioProfesor_Load(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(100, 50, 50, 50);
            lblView.Visible = false;
            btnView.Visible = false;
            lblUser.Text = conexion.nombre(correo2);
            lblTest.Text = "";
            label2.TextAlign = ContentAlignment.MiddleCenter;


            string perfil = conexion.idPerfil(correo2);
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

            dgvCursos.DataSource = conexion.cargarDatos("select Nombre,Clave,Profesor,Escuela, cant_personas from cursos;");
            dgvCursos.Columns[0].HeaderText = "NAME";
            dgvCursos.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvCursos.Columns[0].Width = 170;
            dgvCursos.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvCursos.Columns[1].HeaderText = "KEY";
            dgvCursos.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvCursos.Columns[1].Width = 62;
            dgvCursos.Columns[2].HeaderText = "TEACHER";
            dgvCursos.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvCursos.Columns[2].Width = 150;
            dgvCursos.Columns[3].HeaderText = "SCHOOL";
            dgvCursos.Columns[4].HeaderText = "LIMIT";
            dgvCursos.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCursos.ClearSelection();

            btnBoard.Enabled = false;
            flagRefresh = false;
            txtTitle.Enabled = false;
            txtKey2.Enabled = false;
            txtLimit.Enabled = false;
            txtTeacher.Enabled = false;
            txtSchool.Enabled = false;
            btnOk.Visible = false;
            
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Boolean key = false, title = false, teacher = false, school = false, limit = false;
            if(operacion == 1)//Si la operacion es añadir o crear.
            {
                
                if (txtKey2.Text.Trim() == "")
                {
                    errorProvider.SetError(txtKey2, "Please write the course key");
                }
                else
                {
                    errorProvider.Clear();
                    key = true;
                }
                if (txtTitle.Text.Trim() == "")
                {
                    errorProvider.SetError(txtTitle, "Please write the course title");
                }
                else
                {
                    errorProvider.Clear();
                    title = true;
                }
                if (txtTeacher.Text.Trim() == "")
                {
                    errorProvider.SetError(txtTeacher, "Please write your name as teacher");
                }
                else
                {
                    errorProvider.Clear();
                    teacher = true;
                }
                if (txtSchool.Text.Trim() == "")
                {
                    errorProvider.SetError(txtSchool, "Please write the course school");
                }
                else
                {
                    errorProvider.Clear();
                    school = true;
                }
                if (txtLimit.Text.Trim() == "")
                {
                    errorProvider.SetError(txtLimit, "Please write the limit of people");
                }
                else
                {
                    errorProvider.Clear();
                    limit = true;
                }

                //add/añadir curso solo si los campos contienen texto
                if (key == true && title == true && teacher == true && school == true && limit == true)
                {                  
                    if (conexion.validarCurso(txtTitle.Text, correo2) == true)
                    {
                        MessageBox.Show("You have already created this course");
                    }//Si no existe el curso en la lista...
                    if (conexion.validarCurso(txtTitle.Text, correo2) == false)
                    {   
                        if (conexion.ejecucion("Insert into cursos(Clave,Nombre,Profesor,Escuela,cant_personas) values ('" + txtKey2.Text + "','" + txtTitle.Text + "','" + txtTeacher.Text + "','" + txtSchool.Text + "','" + txtLimit.Text + "')"))
                        {
                            MessageBox.Show("Course succesfully created");
                        }
                        nombreProfesor = conexion.nombre(correo2);
                        cur = Convert.ToInt32(conexion.countCursosProfesor(nombreProfesor));
                        string query = "update perfil set Cursos='" + cur + "' where Correo='" + correo2 + "'";
                        conexion.ejecucion(query);
                        lblTest.Text = "Course *'" + txtTitle.Text + "'* successfully added";
                        string query2 = "insert into listacursosid(Nombre,correoPerfil) values('" + txtTitle.Text.ToString() + "','" + correo2 + "')";
                        conexion.ejecucion(query2);
                        dgvCursos.DataSource = conexion.cargarDatos("select Nombre,Clave,Profesor,Escuela, cant_personas from cursos;");
                        dgvCursos.ClearSelection();

                    } 
                }
            }
            if (flag2 == true || operacion == 2)//Si la operacion es editar.
                {
                    //MessageBox.Show("title:" + txtTitle.Text);
                    DialogResult resp = MessageBox.Show("You are going to modify a course. \n Are you sure?", "COURSE", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (resp == DialogResult.Yes)
                    {
                        string query = "update cursos set Clave='" + txtKey2.Text + "', Nombre='" + txtTitle.Text + "', Profesor='" + txtTeacher.Text + "', Escuela='" + txtSchool.Text + "', cant_personas='" + txtLimit.Text + "' where Nombre='"+nombre+"'";
                        if (conexion.ejecucion(query))
                        {
                            string query2 = "update listacursosid set Nombre='" + txtTitle.Text + "' where Nombre='" + nombre + "'";
                            conexion.ejecucion(query2);
                            MessageBox.Show("Course successfully modified");       
                            dgvCursos.ClearSelection();
                        }
                    }
                }
                txtKey2.Text = "";
                txtTitle.Text = "";
                txtTeacher.Text = "";
                txtSchool.Text = "";
                txtLimit.Text = "";
                txtTitle.Enabled = false;
                txtKey2.Enabled = false;
                txtLimit.Enabled = false;
                txtTeacher.Enabled = false;
                txtSchool.Enabled = false;
            if(flagRefresh == true)
            {
                dgvCursos.DataSource = conexion.cargarYourCourses("select Nombre from listacursosid where correoPerfil='" + correo2 + "';");
            }
            else
            {
                dgvCursos.DataSource = conexion.cargarDatos("select Nombre,Clave,Profesor,Escuela, cant_personas from cursos;");
            }
                
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            operacion = 3;
            btnOk.Visible = true;
            //Si no eres dueño del curso o cursos...
            if (dgvCursos.SelectedRows.Count > 0)
            {
                if (conexion.validarCurso(nombre, correo2) == false)
                {
                    MessageBox.Show("You cannot do any operation with this course");
                }
                else
                {
                    if (operacion == 3)//Si la operacion es eliminar.
                    {
                        DialogResult resp = MessageBox.Show("You're going to delete "+nombre+"\n Are you sure?", "COURSE", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (resp == DialogResult.Yes)
                        {
                            string query = "delete from cursos where Nombre='" + nombre + "'";
                            if (conexion.ejecucion(query))
                            {
                                MessageBox.Show("Course deleted");
                            }
                            string query3 = "delete from listacursosid where Nombre='" + nombre + "'";
                            conexion.ejecucion(query3);
                            cur = cur - 1;
                            string query2 = "update perfil set Cursos='" + cur + "' where Correo='" + correo2 + "'";
                            conexion.ejecucion(query2);
                            dgvCursos.ClearSelection();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("First click on a course to select it");
            }
                
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            flag2 = true;
            operacion = 2;
            txtTitle.Enabled = true;
            txtLimit.Enabled = true;
            txtSchool.Enabled = true;
            btnOk.Visible = true;

            //Si no eres dueño del curso o cursos y seleccionas un curso a modificar...
            if (dgvCursos.SelectedRows.Count > 0)
            {
                if (conexion.validarCurso(nombre, correo2) == false)
                {
                    MessageBox.Show("You cannot do any operation with this course");
                    dgvCursos.DataSource = conexion.cargarDatos("select Nombre,Clave,Profesor,Escuela, cant_personas from cursos;");
                }
                else
                {
                    dgvCursos.DataSource = conexion.cargarDatos("select Nombre,Clave,Profesor,Escuela, cant_personas from cursos;");
                    txtTitle.Text = dgvCursos.Rows[index].Cells[0].Value.ToString();
                    txtKey2.Text = dgvCursos.Rows[index].Cells[1].Value.ToString();
                    txtTeacher.Text = dgvCursos.Rows[index].Cells[2].Value.ToString();
                    txtSchool.Text = dgvCursos.Rows[index].Cells[3].Value.ToString();
                    txtLimit.Text = dgvCursos.Rows[index].Cells[4].Value.ToString();
                    if(flagRefresh == true)
                    {
                        txtTitle.Text = conexion.nombreCurso(conexion.idCursos(nombre).ToString(), conexion.nombre(correo2));
                        txtKey2.Text = conexion.claveCurso(nombre, conexion.nombre(correo2));
                        txtTeacher.Text = conexion.nombre(correo2);
                        txtSchool.Text = conexion.escuelaCurso(nombre, conexion.nombre(correo2));
                        txtLimit.Text = conexion.limiteCurso(nombre, conexion.nombre(correo2));
                        dgvCursos.Columns[0].Visible = false;
                    }

                    

                }
            }
            else
            {
                MessageBox.Show("First click on a course to select it");
            }



        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            flag2 = false;
            operacion = 1; //añadir        
            txtTitle.Enabled = true;
            txtTitle.Text = "";
            txtKey2.Enabled = false;
            txtKey2.Text = "";
            txtLimit.Enabled = true;
            txtLimit.Text = "";
            txtTeacher.Enabled = false;
            txtTeacher.Text = conexion.nombre(correo2);
            txtSchool.Enabled = true;
            txtSchool.Text = "";
            btnOk.Visible = true;
            if(flagRefresh == true)
            {
                dgvCursos.Columns[0].Visible = false;
            }
            else
            {
                dgvCursos.DataSource = conexion.cargarDatos("select Nombre,Clave,Profesor,Escuela, cant_personas from cursos;");
            }
            


        }

        private void btnEvaluation_Click(object sender, EventArgs e)
        {
            Evaluaciones checar = new Evaluaciones();
            checar.cursos = cur;
            checar.correo = correo2;
            checar.teacher = teacher;
            checar.nombreCurso = nombreCurso;
            checar.Show();

            this.Hide();
        }

        private void btnBoard_Click(object sender, EventArgs e)
        {
            Tablon tareas = new Tablon();
            tareas.cursos = cur;
            tareas.correo = correo2;
            tareas.teacher = teacher;
            tareas.nombreCurso = nombre;
            tareas.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Perfil ver = new Perfil();
            ver.cursos = cur;
            ver.correo = correo2;
            ver.teacher = teacher;
            ver.Show();
            this.Hide();
        }

        private void btnCourses_Click(object sender, EventArgs e)//Your courses boton
        {
            flagRefresh = true;
            if(flagRefresh == true)
            {
                
                dgvCursos.DataSource = conexion.cargarYourCourses("select Nombre from listacursosid where correoPerfil='" + correo2 + "';");
                dgvCursos.Columns[0].HeaderText = "YOUR COURSE'S NAME";
                dgvCursos.Columns[0].ReadOnly = true;
                dgvCursos.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvCursos.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvCursos.Columns[0].Width = 531;
                 

            }    
            lblView.Visible = true;
            btnView.Visible = true;
            lblTest.Text = "Here's the list of all of your courses.";

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //flagRefresh = false;
            txtKey2.Text = "";
            txtTitle.Text = "";
            txtTeacher.Text = "";
            txtSchool.Text = "";
            txtLimit.Text = "";
            txtTitle.Enabled = false;
            txtKey2.Enabled = false;
            txtLimit.Enabled = false;
            txtTeacher.Enabled = false;
            txtSchool.Enabled = false;
            btnOk.Visible = false;
            btnBoard.Enabled = false;
            if (flagRefresh == true)
            {
                dgvCursos.DataSource = conexion.cargarYourCourses("select Nombre from listacursosid where correoPerfil='" + correo2 + "';");
                dgvCursos.ClearSelection();
                dgvComing.ClearSelection();
            }
            else
            {
                dgvCursos.DataSource = conexion.cargarDatos("select Nombre,Clave,Profesor,Escuela, cant_personas from cursos;");
                dgvCursos.Columns[0].HeaderText = "NAME";
                dgvCursos.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvCursos.Columns[0].Width = 170;
                dgvCursos.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvCursos.Columns[1].HeaderText = "KEY";
                dgvCursos.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvCursos.Columns[1].Width = 62;
                dgvCursos.Columns[2].HeaderText = "TEACHER";
                dgvCursos.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvCursos.Columns[2].Width = 150;
                dgvCursos.Columns[3].HeaderText = "SCHOOL";
                dgvCursos.Columns[4].HeaderText = "LIMIT";
                dgvCursos.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvCursos.ClearSelection();
                dgvComing.ClearSelection();
            }
            
            
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            flagRefresh = false;
            lblView.Visible = false;
            btnView.Visible = false;
            //Para cuando regrese con FirstView se note el formato del dataview de Cursos y no de Coming soon
            dgvCursos.DataSource = conexion.cargarDatos("select Nombre,Clave,Profesor,Escuela, cant_personas from cursos;");
            dgvCursos.Columns[0].HeaderText = "NAME";
            dgvCursos.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvCursos.Columns[0].Width = 170;
            dgvCursos.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvCursos.Columns[1].HeaderText = "KEY";
            dgvCursos.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvCursos.Columns[1].Width = 62;
            dgvCursos.Columns[2].HeaderText = "TEACHER";
            dgvCursos.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvCursos.Columns[2].Width = 150;
            dgvCursos.Columns[3].HeaderText = "SCHOOL";
            dgvCursos.Columns[4].HeaderText = "LIMIT";
            dgvCursos.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCursos.ClearSelection();
            dgvComing.ClearSelection();
            lblAdd.Visible = true;
            btnAdd.Visible = true;
        }

        

        public bool validNum(Char c)
        {
            if (c >= '0' && c <= '9')
            {
                return true;
            }
            return false;
        }

        private void txtKey2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (validNum(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (validNum(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void lblKey_DoubleClick(object sender, EventArgs e)//GEnerate course key
        {
            //rnd = new Random();
            txtKey2.Text = rnd.Next(10000,50000).ToString();
        }

        

        private void pbkey_DoubleClick(object sender, EventArgs e)
        {
            txtKey2.Text = rnd.Next(10000, 50000).ToString();
        }

        private void dgvComing_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvCursos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            label9.Visible = false;
            index = dgvCursos.CurrentRow.Index;   
            if(flagRefresh == true)
            {
                nombre = dgvCursos.Rows[index].Cells[0].Value.ToString();
            }
            else
            {
                nombre = dgvCursos.Rows[index].Cells[0].Value.ToString();
                clave = dgvCursos.Rows[index].Cells[1].Value.ToString();
                //Solo lectura del DataGridView
                dgvCursos.Rows[index].Cells[0].ReadOnly = true;
                dgvCursos.Rows[index].Cells[1].ReadOnly = true;
                dgvCursos.Rows[index].Cells[2].ReadOnly = true;
                dgvCursos.Rows[index].Cells[3].ReadOnly = true;
                dgvCursos.Rows[index].Cells[4].ReadOnly = true;
            }
            //contar la cantidad de personas que existen en el curso
            
            
            if (dgvCursos.SelectedRows.Count > 0)
            {
                flag = true;
                btnBoard.Enabled = true;
            }
            nombreProfesor = conexion.nombre(correo2);
            //Mostrar comentarios y fecha para Coming Soon
            if (conexion.validarCurso(nombre, correo2) == true)
            {
                label2.Text = "COMING SOON " + nombre;
                dgvComing.DataSource = conexion.cargarDatos2("select Publicacion,FechaProxima from tablon where idCursos='" + conexion.idCursosProfesor(nombreProfesor,nombre) + "'");
                dgvComing.Columns[0].HeaderText = "POST";
                dgvComing.Columns[1].HeaderText = "DATE LIMIT";
                dgvComing.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvComing.Columns[0].Width = 128;
                dgvComing.Columns[0].ReadOnly = true;
                dgvComing.Columns[1].ReadOnly = true;
                dgvComing.ClearSelection();
            }
            else if (conexion.validarCurso(nombre, correo2) == false)
            {
                btnBoard.Enabled = false;
                MessageBox.Show("You don't own this course");
                dgvCursos.DataSource = conexion.cargarDatos("select Nombre,Clave,Profesor,Escuela, cant_personas from cursos;");
            }
            if (flagRefresh == false)
            {
                dgvCursos.DataSource = conexion.cargarDatos("select Nombre,Clave,Profesor,Escuela, cant_personas from cursos;");
            }
            else if (flagRefresh == true)
            {
                dgvCursos.DataSource = conexion.cargarYourCourses("select Nombre from listacursosid where correoPerfil='" + correo2 + "';");

            }
        }

        

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.ForwardDiagonal);
            mgraficos.FillRectangle(lgb, area);
            mgraficos.DrawRectangle(pen, area);
        }

        

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraficos = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(96, 150, 50), 1);

            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(220, 20, 60), Color.FromArgb(0, 139, 139), LinearGradientMode.Horizontal);
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
        private void panel2_DoubleClick(object sender, EventArgs e)
        {

        }


    }
}
