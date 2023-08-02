using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace Droplearn
{
    class Conexion2
    {
        public MySqlConnection conexion;
        public String strConex = "server=127.0.0.1; user id=root; password=; database=droplearn; SslMode=none";
        public DataTable dt = new DataTable();

        public bool abrir()
        {
            try
            {
                conexion = new MySqlConnection(strConex);
                conexion.Open();
                dt.Clear();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo hacer conexion con la BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable cargarDatos(String query)
        {
            try
            {
                abrir();
                MySqlDataAdapter daDatos = new MySqlDataAdapter(query, conexion);
                daDatos.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo hacer conexion con la BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }

        public bool ejecucion(String query)
        {
            try
            {

                abrir();
                MySqlCommand cmdQuery = new MySqlCommand(query, conexion);
                cmdQuery.ExecuteNonQuery();
                conexion.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo hacer conexion con la BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
