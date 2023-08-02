using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Droplearn {
    
    

    class Conexion
    {
        public string correo;
        public string correo3;
        public string id;
        public int flag = 0;
        public MySqlConnection conex;
        public String strConexion = "server=127.0.0.1; user id=root; password=; database=droplearn; SslMode=none; Min Pool Size=10; Max Pool Size=100; Pooling=false";
        public DataTable dt = new DataTable();
        public DataTable dt2 = new DataTable();
        public DataTable dt3 = new DataTable();



        public bool abrir()
        {
            try
            {
                conex = new MySqlConnection(strConexion);
                conex.Open();
                dt.Clear();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo hacer conexion con la BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

       public  MySqlConnection conectando()//refres perfil
        {
            
            try
            {
                conex = new MySqlConnection(strConexion);
               
                return conex;
            }catch(MySqlException ex)
            {
                Console.Write("Error: " + ex.Message);
                return null;
            }
            
        }

        public DataTable cargarDatos(String query)
        {
            try
            {

                abrir();
                MySqlDataAdapter daDatos = new MySqlDataAdapter(query, conex);
                daDatos.Fill(dt);
                conex.Close();
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo hacer conexion con la BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }
        public DataTable cargarDatos2(String query)
        {
            try
            {            
                abrir();
                dt2.Clear();
                MySqlDataAdapter daDatos = new MySqlDataAdapter(query, conex);
                daDatos.Fill(dt2);
                conex.Close();
                return dt2;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo hacer conexion con la BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt2;
            }
        }
        public DataTable cargarYourCourses(String query)
        {
            try
            {
                abrir();
                dt3.Clear();
                MySqlDataAdapter daDatos = new MySqlDataAdapter(query, conex);
                daDatos.Fill(dt3);
                conex.Close();
                return dt3;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo hacer conexion con la BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt3;
            }
        }


        public bool ejecucion(String query)
        {
            try
            {
                
                abrir();
                MySqlCommand cmdQuery = new MySqlCommand(query, conex);
                cmdQuery.ExecuteNonQuery();
                conex.Close();

                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo hacer conexion con la BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public string perfil(String query)
        {
            string x;
            abrir();
            MySqlCommand cmd = new MySqlCommand(query, conex);
            x = cmd.ExecuteScalar().ToString();
            conex.Close();
            return x;
        }

       

        public void logear(String correo, String contraseña)
        {
            
            try
            {
                abrir();
                MySqlCommand cmd = new MySqlCommand("SELECT Nombre, TipodeUsuario FROM registro WHERE Correo = @correo AND Contraseña = @contraseña",conex);
                cmd.Parameters.AddWithValue("correo", correo);
                cmd.Parameters.AddWithValue("contraseña", contraseña);
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                flag = 0;

                
                if (dt.Rows.Count == 1)
                {
                    if(dt.Rows[0][1].ToString() == "Student")
                    {
                        InicioEstudiante inicio = new InicioEstudiante();
                        flag = 1;
                        string query = "select Cursos from perfil where Correo='" + correo + "'";
                        MySqlCommand cmd2 = new MySqlCommand(query, conex);
                        int cursillo = Convert.ToInt32(cmd2.ExecuteScalar());
                        inicio.cur = cursillo;
                        inicio.correo2 = correo;
                        inicio.student = dt.Rows[0][1].ToString();
                        inicio.Show();
                    }
                    else if(dt.Rows[0][1].ToString() == "Teacher")
                    {
                        InicioProfesor inicioProfesor = new InicioProfesor();
                        flag = 1;
                        string query = "select Cursos from perfil where Correo='" + correo + "'";
                        MySqlCommand cmd2 = new MySqlCommand(query, conex);
                        int cursillo = Convert.ToInt32(cmd2.ExecuteScalar());
                        inicioProfesor.cur = cursillo;
                        inicioProfesor.correo2 = correo;
                        inicioProfesor.teacher = dt.Rows[0][1].ToString();
                        inicioProfesor.Show();
                    }
                }
                else
                {
                    flag = 0;
                    MessageBox.Show("Usuario y/o Contraseña Incorrecta");
                }
                conex.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool validarCurso(String c, String correo)// c = nombre del curso
        {
                abrir();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM listacursosid WHERE Nombre = @c AND correoPerfil= @correo", conex);
                cmd.Parameters.AddWithValue("c", c);
                cmd.Parameters.AddWithValue("correo", correo);
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                flag = 0;
                if (dt.Rows.Count >= 1)
                {
                //MessageBox.Show("Si existe");
                    return true;
                }
            //MessageBox.Show("No existe");
            return false;
            
        }
        public bool validarEvaluacion(String correo)
        {
            abrir();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM evaluaciones WHERE Correo = @correo", conex);
            cmd.Parameters.AddWithValue("correo", correo);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            flag = 0;
            if (dt.Rows.Count >= 1)
            {
                //MessageBox.Show("Si existe");
                return true;
            }
            //MessageBox.Show("No existe");
            return false;
            conex.Close();
        }

        public bool validarEvaluacion2(String correo, String nombre)
        {
            abrir();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM evaluaciones WHERE Correo = @correo and Nombre = @nombre", conex);
            cmd.Parameters.AddWithValue("correo", correo);
            cmd.Parameters.AddWithValue("nombre", nombre);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            flag = 0;
            if (dt.Rows.Count >= 1)
            {
                //MessageBox.Show("Si existe");
                return true;
            }
            //MessageBox.Show("No existe");
            return false;
            conex.Close();
        }



        public int idReg()
        {
            abrir();
            string query = "select MAX(idRegistro) AS LASTID from registro";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            int lastId = Convert.ToInt32(cmd.ExecuteScalar());
            conex.Close();
            return lastId;
        }
        /*
        public int idReg()
        {
            abrir();
            string query = "select count(*) from registro";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            int lastId = Convert.ToInt32(cmd.ExecuteScalar());
            return lastId;
        }*/
        public int cuentaCursos(String nombreCurso)//Contar hasta el limite de personas registradas en un curso
        {
            abrir();
            string query = "select count(*) from listacursosid where Nombre='"+nombreCurso+"'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            conex.Close();
            return count;
        }

        public int cantPersonas(String nombreCurso)//Para obtener el limite de cantidad de personas de un curso y definir el stop de insert.
        {
            abrir();
            string query = "select cant_personas from cursos where Nombre='" + nombreCurso + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            int cantP = Convert.ToInt32(cmd.ExecuteScalar());
            conex.Close();
            return cantP;
        }
        public int idCursos(String nombreCurso)
        {
            abrir();
            string query = "select idCursos from cursos where Nombre='"+nombreCurso+"'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            int idCur = Convert.ToInt32(cmd.ExecuteScalar());
            conex.Close();
            return idCur;
        }
        public int idCursosProfesor(String nombreProfesor, String nombreCurso)//idCursos del nombre y profesor seleccionado del datagridview cursos// resultado = 1 curso en especifico
        {
            abrir();
            string query = "select idCursos from cursos where Profesor='" + nombreProfesor + "' and Nombre='"+nombreCurso+"'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            int idCur = Convert.ToInt32(cmd.ExecuteScalar());
            conex.Close();
            return idCur;
        }

        public string nombre(string correo)//Nombre de usuario
        {
            abrir();
            string query = "select Nombre from registro where Correo='" + correo + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string name = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return name;
        }
        public string nombreProfesor(string nombreProfesor,string nombreCurso)//SAcar nombre del profesor a traves del curso
        {
            abrir();
            string query = "select Profesor from cursos where Nombre='" + nombreCurso + "' and Profesor='"+nombreProfesor+"'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string np = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return np;
        }
        public string nombreProfesor2(string nombreCurso)//Sacar nombre del profesor solamente con el nombre del curso
        {
            abrir();
            string query = "select Profesor from cursos where Nombre='" + nombreCurso + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string np2 = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return np2;
        }
        public string Escolaridad(string correo)
        {
            abrir();
            string query = "select Escolaridad from perfil where Correo='" + correo + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string esco = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return esco;
        }
        public string countCursosProfesor(string nombreProfesor)//Obtener la cantidad de actividades entregadas de un estudiante por medio de su correo y el idtablon.
        {
            abrir();
            string query = "select count(*) from cursos where Profesor='"+nombreProfesor+"'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string ccp = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return ccp;
        }

        public string Cursos(string correo)
        {
            abrir();
            string query = "select Cursos from perfil where Correo='" + correo + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string esco = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return esco;
        }

        public string NiveldeEstudios(string correo)
        {
            abrir();
            string query = "select NiveldeEstudios from perfil where Correo='" + correo + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string esco = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return esco;
        }

        public string Edad(string correo)
        {
            abrir();
            string query = "select Edad from perfil where Correo='" + correo + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string esco = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return esco;
        }


        public string Imagen(string correo)
        {
            abrir();
            string query = "select imagen from perfil where Correo='" + correo + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string esco = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return esco;
        }



        public string idPerfil(string correo)
        {
            abrir();
            string query = "select idPerfil from perfil where Correo='" + correo + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string esco = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return esco;
        }

        public string nombreCurso(string idCurso,string profesor)//Sacar el nombre de un curso de un profesor en especifico
        {
            abrir();
            string query = "select Nombre from cursos where Profesor='" + profesor + "' and idCursos = '"+idCurso+"'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string nc = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return nc;
        }
        public string claveCurso(string nombreCurso, string profesor)//Sacar la clave de un curso de un profesor en especifico
        {
            abrir();
            string query = "select Clave from cursos where Nombre='" + nombreCurso + "' and Profesor='"+profesor+"'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string cc = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return cc;
        }
        public string escuelaCurso(string nombreCurso, string profesor)//Sacar la escuela de un curso de un profesor en especifico
        {
            abrir();
            string query = "select Escuela from cursos where Nombre='" + nombreCurso + "' and Profesor='" + profesor + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string ec = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return ec;
        }
        public string limiteCurso(string nombreCurso, string profesor)//Sacar la escuela de un curso de un profesor en especifico
        {
            abrir();
            string query = "select cant_personas from cursos where Nombre='" + nombreCurso + "' and Profesor='" + profesor + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string lc = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return lc;
        }
        //Tablon
        public string idTablon(string publicacion, string idCursos)//Sacar la escuela de un curso de un profesor en especifico
        {
            abrir();
            string query = "select idTablon from tablon where Publicacion='" + publicacion + "' and idCursos='" + idCursos + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string it = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return it;
        }
        //Para sacar el total de actividades de un mismo curso
        public string tablonActividades(string idCursos)//Obtener la cantidad de publicaciones o actividades puestas en un solo curso
        {
            abrir();
            string query = "select count(*) from tablon where idCursos='" + idCursos + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string ta = Convert.ToString(cmd.ExecuteScalar());
            conex.Close();
            return ta;
        }
        //Evaluaciones
        public int actividades(string correo, string idCursos)
        {
            abrir();
            string query = "select Actividades from evaluaciones where Correo='" + correo + "' and idCursos='"+idCursos+"'";
            MySqlCommand cmd2 = new MySqlCommand(query, conex);
            int act = Convert.ToInt32(cmd2.ExecuteScalar());
            conex.Close();
            return act;
        }
        public int progreso(string correo, string idCursos)
        {
            abrir();
            string query = "select Progreso from evaluaciones where Correo='" + correo + "' and idCursos='" + idCursos + "'";
            MySqlCommand cmd2 = new MySqlCommand(query, conex);
            int act = Convert.ToInt32(cmd2.ExecuteScalar());
            return act;
        }
        //Trabajos
        public string trabajosActividades(string correo, String idCursosTrabajos)//Obtener la cantidad de actividades entregadas de un estudiante por medio de su correo y el idtablon.
        {
            abrir();
            string query = "select count(*) from tablon inner join trabajos on tablon.idTablon = trabajos.idTablon where tablon.idCursos='" + idCursosTrabajos + "' and trabajos.Correo='" + correo + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string ta = Convert.ToString(cmd.ExecuteScalar());
            return ta;
        }
        public string idCursosTrabajos(string correo,string idTablon)//Obtener la cantidad de actividades entregadas de un estudiante por medio de su correo y el idtablon.
        {
            abrir();
            string query = "select idCursos from tablon inner join trabajos on tablon.idTablon = trabajos.idTablon where trabajos.Correo='" + correo + "' and trabajos.idTablon='" + idTablon + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string ta = Convert.ToString(cmd.ExecuteScalar());
            return ta;
        }

        public string trabajosDescripcion(string nombreTrabajo)//Obtener la cantidad de actividades entregadas de un estudiante por medio de su correo y el idtablon.
        {
            abrir();
            string query = "select Descripcion from trabajos where Nombre='" + nombreTrabajo+ "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string td = Convert.ToString(cmd.ExecuteScalar());
            return td;
        }
        public string nombreListaCursos(string correo)//Obtener nombre del curso donde este el profesor
        {
            abrir();
            string query = "select Nombre from listacursosid where correoPerfil='" + correo + "'";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            string nlc = Convert.ToString(cmd.ExecuteScalar());
            return nlc;
        }
        public bool validarTrabajos(String idTablon, String correo)//validar trabajos para que cuando se inserte un trabajo, solo se pueda insertar 1 dependiendo de la id de publicacion
        {
            abrir();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM trabajos WHERE idTablon = @idTablon and Correo = @correo", conex);
            cmd.Parameters.AddWithValue("idTablon", idTablon);
            cmd.Parameters.AddWithValue("correo", correo);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            flag = 0;
            if (dt.Rows.Count >= 1)
            {
                //MessageBox.Show("Si existe");
                return true;
            }
            //MessageBox.Show("No existe");
            return false;

        }
        public bool validarActividades(String idTablon, String idCursos,String correo)//validar actividades para que cuando se haga la modificacion de evaluaciones, solo identifique las actividades de un curso y las separe de otro
        {
            abrir();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM trabajos WHERE idTablon = @idTablon and Correo = @correo", conex);
            cmd.Parameters.AddWithValue("idTablon", idTablon);
            cmd.Parameters.AddWithValue("correo", correo);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            flag = 0;
            if (dt.Rows.Count >= 1)
            {
                //MessageBox.Show("Si existe");
                return true;
            }
            //MessageBox.Show("No existe");
            return false;
            //obtener el idTablon con idCursos, y entonces conexion.Actividades(idTablon) agarra 
        }
    }
}
