using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Newtonsoft.Json;
using System.Net.Mail;




namespace WSUsuario
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class WSUsuario : IWSUsuario
    {
        string stringConexion = ConfigurationManager.ConnectionStrings["oracle"].ConnectionString;

        public Usuario buscarUsuario(string id)
        {
            Usuario uUsuario = new Usuario();
            try {
            } catch (Exception ex) {
                throw new Exception("Error al Buscar Usuario en la Base de Datos");

            }
            return uUsuario;
        }

        public Boolean validarLogin(string id, string pass)
        {
            bool valido = false;
            try {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT USUARIO_NOMBRECOMPLETO FROM USUARIO WHERE USUARIO_ID='"+id+"' and USUARIO_PASSWORD='"+pass+"'";

                OracleDataReader dtreader = cmd.ExecuteReader();

                while (dtreader.Read())
                {
                    valido = true;

                }

            } catch (Exception ex) {
                throw new Exception("Error al validar a usuario");
            }

            return valido;

        }
        public String obtenerUsuario(String id) {
            string nombreCompleto = "";
            try {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select USUARIO_NOMBRECOMPLETO from DBTICKETREST.USUARIO where USUARIO_ID='" + id + "'";

                OracleDataReader dtreader = cmd.ExecuteReader();

                while (dtreader.Read())
                {

                    nombreCompleto = dtreader.GetString(0);
                }

            } catch {
                throw new Exception("Error, No se Encuentra Registro");
            }

            return nombreCompleto;
        }

        public void agregarUsuario(String id, String nomCom, String pass, String correo, int saldo, String tipo, int cargo_id, String vale_ext)
        {
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("AGREGAR_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("USUARIO_ID", id);
                cmd.Parameters.Add("USUARIO_NOMBRECOMPLETO", nomCom);
                cmd.Parameters.Add("USUARIO_PASSWORD", pass);
                cmd.Parameters.Add("USUARIO_CORREO", correo);
                cmd.Parameters.Add("USUARIO_SALDO", saldo);
                cmd.Parameters.Add("USUARIO_TIPO", tipo);
                cmd.Parameters.Add("CARGO_CARGO_ID", cargo_id);
                cmd.Parameters.Add("USUARIO_VALE_EXT", vale_ext);

                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
        }
        public Boolean modificarUsuario(String id, String nomCom, String pass, String correo, int saldo, String tipo, int cargo_id, String vale_ext)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("MODIFICAR_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("USUARIO_ID", id);
                cmd.Parameters.Add("USUARIO_NOMBRECOMPLETO", nomCom);
                cmd.Parameters.Add("USUARIO_PASSWORD", pass);
                cmd.Parameters.Add("USUARIO_CORREO", correo);
                cmd.Parameters.Add("USUARIO_SALDO", saldo);
                cmd.Parameters.Add("USUARIO_TIPO", tipo);
                cmd.Parameters.Add("CARGO_CARGO_ID", cargo_id);
                cmd.Parameters.Add("USUARIO_VALE_EXT", vale_ext);
                boolAux = true;
                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            return boolAux;

        }
        public Boolean eliminarUsuario(String id)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("ELIMINAR_USUARIO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("USUARIO_ID", id);
                cmd.ExecuteNonQuery();
                boolAux = true;
                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            return boolAux;
        }
        public String listarProductos()
        {
            List<Producto> listAux = new List<Producto>();

            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select PRODUCTO_ID, PRODUCTO_NOMBRE, PRODUCTO_VALOR FROM PRODUCTOS";

                conn.Open();

                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {
                        Producto prod = new Producto
                        {
                            _productoId = Convert.ToInt32(dtreader[0]),
                            _productoNombre = dtreader[1].ToString(),
                            _productoValor = Convert.ToInt32(dtreader[2])

                        };
                        listAux.Add(prod);

                    }
                }


                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            var json = JsonConvert.SerializeObject(listAux);
            return json;
        }
        public Boolean modificarProds(int id, String nomProd, int valorProd)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("MODIFICAR_PRODUCTO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PRODUCTO_ID", id);
                cmd.Parameters.Add("PRODUCTO_NOMBRE", nomProd);
                cmd.Parameters.Add("USUARIO_VALOR", valorProd);

                boolAux = true;
                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            return boolAux;

        }
        public String listarMenu()
        {
            List<Menu> listAux = new List<Menu>();

            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select MENU_ID, MENU_NOMBRE, TURNO_TURNO_ID FROM MENU";

                conn.Open();

                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {
                        Menu menu = new Menu
                        {
                            _menuId = Convert.ToInt32(dtreader[0]),
                            _menuNombre = dtreader[1].ToString(),
                            _turnoId = Convert.ToInt32(dtreader[2])

                        };
                        listAux.Add(menu);

                    }
                }


                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            var json = JsonConvert.SerializeObject(listAux);
            return json;
        }
        public Boolean modificarMenu(int id, String nomMenu, int turnoId, int p1_id, int p2_id, int p3_id, int p4_id, int n_p1_id, int n_p2_id, int n_p3_id, int n_p4_id)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("MODIFICAR_MENU", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("MENU_ID", id);
                cmd.Parameters.Add("MENU_NOMBRE", nomMenu);
                cmd.Parameters.Add("TURNO_TURNO_ID", turnoId);
                cmd.Parameters.Add("P1_ID", p1_id);
                cmd.Parameters.Add("P2_ID", p2_id);
                cmd.Parameters.Add("P3_ID", p3_id);
                cmd.Parameters.Add("P4_ID", p4_id);
                cmd.Parameters.Add("NEWP1_ID", n_p1_id);
                cmd.Parameters.Add("NEWP2_ID", n_p2_id);
                cmd.Parameters.Add("NEWP3_ID", n_p3_id);
                cmd.Parameters.Add("NEWP4_ID", n_p4_id);

                boolAux = true;
                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, Al Modificar" + ex.Message);
            }
            return boolAux;

        }
        public String listarTurnos()
        {
            List<Turno> listAux = new List<Turno>();
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select TURNO_ID, TURNO_NOMBRE, MASTER_TURNOCO_COLACION_COD FROM TURNO";
                conn.Open();
                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {
                        Turno turno = new Turno
                        {
                            _turnoId = Convert.ToInt32(dtreader[0]),
                            _turnoNom = dtreader[1].ToString(),
                            _masterturnoColacionCod = Convert.ToInt32(dtreader[2])
                        };
                        listAux.Add(turno);
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            var json = JsonConvert.SerializeObject(listAux);
            return json;
        }
        public Boolean modificarTurno(int id, String nomProd, int masterTurnoCol)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("MODIFICAR_TURNOS", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("TURNO_ID", id);
                cmd.Parameters.Add("TURNO_NOMBRE", nomProd);
                cmd.Parameters.Add("MASTER_TURNOCO_COLACION_COD", masterTurnoCol);

                boolAux = true;
                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, Al Modificar Turno" + ex.Message);
            }
            return boolAux;

        }
        public Boolean agregarTurno(String nomProd, int mtco)
        {
            Boolean bolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("AGREGAR_TURNO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("TURNO_NOMBRE", nomProd);
                cmd.Parameters.Add("MASTER_TURNOCO_COLACION_COD", mtco);

                cmd.ExecuteNonQuery();

                conn.Close();
                bolAux = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }

            return bolAux;
        }
        public Boolean eliminarProd(int id)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("ELIMINAR_PRODUCTO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PRODUCTO_ID", id);
                cmd.ExecuteNonQuery();
                boolAux = true;
                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, AL Eliminar Producto" + ex.Message);
            }
            return boolAux;
        }
        public Boolean eliminarMenu(int id)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("ELIMINAR_MENU", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("MENU_ID", id);
                cmd.ExecuteNonQuery();
                boolAux = true;
                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            return boolAux;
        }
        public Boolean eliminarTurno(int id)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("ELIMINAR_TURNO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("TURNO_ID", id);
                cmd.ExecuteNonQuery();
                boolAux = true;
                conn.Close();


            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            return boolAux;
        }
        public void agregarProducto(int id, String nomProd, int valor)
        {
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("AGREGAR_PRODUCTO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("PRODUCTO_ID", id);
                cmd.Parameters.Add("PRODUCTO_NOMBRE", nomProd);
                cmd.Parameters.Add("PRODUCTO_VALOR", valor);

                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }


        }
        public Boolean agregarMenu(int id, String nomMenu, int turnoId, int p1_id, int p2_id, int p3_id, int p4_id)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("AGREGAR_MENU", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("MENU_ID", id);
                cmd.Parameters.Add("MENU_NOMBRE", nomMenu);
                cmd.Parameters.Add("TURNO_TURNO_ID", turnoId);
                cmd.Parameters.Add("P1_ID", p1_id);
                cmd.Parameters.Add("P2_ID", p2_id);
                cmd.Parameters.Add("P3_ID", p3_id);
                cmd.Parameters.Add("P4_ID", p4_id);

                boolAux = true;
                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, Al Agregar Menu" + ex.Message);
            }
            return boolAux;

        }
        public Producto ProdsPorId(int id)
        {
            Producto p = new Producto();
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select PRODUCTO_ID, PRODUCTO_NOMBRE, PRODUCTO_VALOR PRODUCTO FROM PRODUCTOS WHERE PRODUCTO_ID='" + id + "'";
                conn.Open();
                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {


                        Producto prod = new Producto
                        {
                            _productoId = Convert.ToInt32(dtreader[0]),
                            _productoNombre = (dtreader[1]).ToString(),
                            _productoValor = Convert.ToInt32(dtreader[2])
                        };
                        p = prod;
                    }

                }

                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }

            return p;
        }
        public List<DetalleMenu> DetalleporId(int id)
        {
            List<DetalleMenu> ListDetMenu = new List<DetalleMenu>();
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select MENU_MENU_ID, PRODUCTOS_PRODUCTO_ID FROM DETALLE_MENU WHERE MENU_MENU_ID='" + id + "'";
                conn.Open();
                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {


                        DetalleMenu det = new DetalleMenu
                        {
                            _idMenu = Convert.ToInt32(dtreader[0]),
                            _prodId = Convert.ToInt32(dtreader[1])
                        };
                        ListDetMenu.Add(det);
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }

            return ListDetMenu;
        }
        public String listTablaMenu()
        {
            List<tablaDetMenu> listAux = new List<tablaDetMenu>();
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select MENU_ID, MENU_NOMBRE, TURNO_TURNO_ID FROM MENU";
                conn.Open();
                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {
                        int menuId = Convert.ToInt32(dtreader[0]);
                        String nomMenu = dtreader[1].ToString();
                        int turno = Convert.ToInt32(dtreader[2]);

                        int p1 = 0;
                        int p2 = 0;
                        int p3 = 0;
                        int p4 = 0;
                        List<DetalleMenu> lDetMenu = DetalleporId(menuId);
                        if (lDetMenu.Count() == 0 || lDetMenu.Count() < 4)
                        {
                            p1 = 0;
                            p2 = 0;
                            p3 = 0;
                            p4 = 0;

                        }
                        else
                        {
                            p1 = Int32.Parse(lDetMenu[0]._prodId.ToString());
                            p2 = Int32.Parse(lDetMenu[1]._prodId.ToString());
                            p3 = Int32.Parse(lDetMenu[2]._prodId.ToString());
                            p4 = Int32.Parse(lDetMenu[3]._prodId.ToString());

                        }




                        var prod1 = ProdsPorId(p1);
                        var prod2 = ProdsPorId(p2);
                        var prod3 = ProdsPorId(p3);
                        var prod4 = ProdsPorId(p4);

                        tablaDetMenu det = new tablaDetMenu
                        {
                            _idMenu = menuId,
                            _nomMenu = nomMenu,
                            _turnoMenu = turno,
                            _prod1 = prod1._productoNombre,
                            _prod2 = prod2._productoNombre,
                            _prod3 = prod3._productoNombre,
                            _prod4 = prod4._productoNombre,
                            _valorProd1 = prod1._productoValor,
                            _valorProd2 = prod2._productoValor,
                            _valorProd3 = prod3._productoValor,
                            _valorProd4 = prod4._productoValor,
                            _totalValor = prod1._productoValor + prod2._productoValor + prod3._productoValor + prod4._productoValor

                        };

                        listAux.Add(det);
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            var json = JsonConvert.SerializeObject(listAux);
            return json;
        }

        public String listarDetalleMenu()
        {
            List<DetalleMenu> listAux = new List<DetalleMenu>();
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select MENU_MENU_ID, PRODUCTOS_PRODUCTO_ID FROM DETALLE_MENU";
                conn.Open();
                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {
                        DetalleMenu DetMenu = new DetalleMenu
                        {
                            _idMenu = Convert.ToInt32(dtreader[0]),
                            _prodId = Convert.ToInt32(dtreader[1])
                        };
                        listAux.Add(DetMenu);
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            var json = JsonConvert.SerializeObject(listAux);
            return json;
        }
        public String listarUsuario()
        {
            List<Usuario> listAux = new List<Usuario>();

            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select USUARIO_ID, USUARIO_NOMBRECOMPLETO, USUARIO_PASSWORD, USUARIO_CORREO, USUARIO_SALDO, USUARIO_TIPO, CARGO.CARGO_VALOR, USUARIO_VALE_EXT from USUARIO JOIN CARGO ON(CARGO.CARGO_ID = USUARIO.CARGO_CARGO_ID)";

                conn.Open();

                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {
                        Usuario usuario = new Usuario
                        {


                            _id = dtreader[0].ToString(),
                            _nombreCompleto = dtreader[1].ToString(),
                            _password = dtreader[2].ToString(),
                            _correo = dtreader[3].ToString(),
                            _saldo = Convert.ToInt32(dtreader[4]),
                            _tipo = dtreader[5].ToString(),
                            _cargoId = Convert.ToInt32(dtreader[6]),
                            _valeExt = dtreader[7].ToString()
                        };
                        listAux.Add(usuario);

                    }
                }


                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            var json = JsonConvert.SerializeObject(listAux);
            return json;
        }

        public String ObtenerTipo(string id, string pass)
        {
            string tipoUser =" ";
            string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select USUARIO_TIPO from USUARIO where USUARIO_ID='" + id + "' and USUARIO_PASSWORD='" + pass + "'";

                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {
                        tipoUser = dtreader.GetString(0);
                    }
                }
                dtreader.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception("Error al validar a usuario "+ ex.Message);
            }
            conn.Close();
            return tipoUser;
        }
        public String[] ObtenerValeTurno(string id)
        {
            string[] auxUser = new String[4];
            string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select VALE_ID,TURNO_NOMBRE,TURNO_ID,valor_totalusuario from USUARIO join VALE on (USUARIO_ID = USUARIO_USUARIO_ID)join Turno on (TURNO_TURNO_ID = TURNO_ID) where USUARIO_ID ='" + id +"'";

                OracleDataReader dtreader = cmd.ExecuteReader();

                if (dtreader.FieldCount>0)
                {
                    while (dtreader.Read())
                    {
                        auxUser[0] = dtreader.GetInt32(0).ToString();
                        auxUser[1] = dtreader.GetString(1);
                        auxUser[2] = dtreader.GetInt32(2).ToString();
                        auxUser[3] = dtreader.GetInt32(3).ToString();
                    }
                }
                dtreader.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception("Error al validar a usuario " + ex.Message);
            }
            conn.Close();
            return auxUser;
        }
        public List<String> ObtenerProducto(string NomMenu)
        {
            List<string> auxUser = new List<string>();
            string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select PRODUCTO_NOMBRE,PRODUCTO_VALOR from Menu me join DETALLE_MENU dt on (me.MENU_ID=dt.MENU_MENU_ID) join PRODUCTOS pr on (dt.PRODUCTOS_PRODUCTO_ID=pr.PRODUCTO_ID)where me.MENU_Nombre='" + NomMenu + "'";

                OracleDataReader dtreader = cmd.ExecuteReader();

                if (dtreader.FieldCount > 0)
                {
                    while (dtreader.Read())
                    {
                        auxUser.Add(dtreader.GetString(0));
                        auxUser.Add(dtreader.GetInt32(1).ToString());
                    }
                }
                dtreader.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception("Error al validar a usuario " + ex.Message);
            }
            conn.Close();
            return auxUser;
        }
        public string[] ObtenerMenu(string idTurno)
        {
            string[] auxUser = new string[3];
            int i = 0;
            string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select Menu_Nombre from Menu join DETALLE_MENU on (MENU_ID=Menu_Menu_ID)where TURNO_TURNO_ID='" + idTurno + "' group by MENU_Nombre";

                OracleDataReader dtreader = cmd.ExecuteReader();

                if (dtreader.FieldCount > 0)
                {
                    while (dtreader.Read())
                    {
                        auxUser[i] = dtreader.GetString(0);
                        i++;
                    }
                }
                dtreader.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception("Error al validar a usuario " + ex.Message);
            }
            conn.Close();
            return auxUser;
        }
        public string[] ImprimirVale(string id)
        {
            string[] auxUser = new string[7];
            string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select EmprsaExt_Nombre,Vale_fecha,to_char(Vale_fecha,'HH24:MI:SS'),vale_id,valor_totalusuario,turno_nombre,usuario_nombrecompleto from vale va join usuario us on(USUARIO_USUARIO_ID=USUARIO_ID) join Empresa_externa ee on (va.Empresa_externa_EmprsaExt_ID=ee.EmprsaExt_ID)join turno tu on (va.Turno_Turno_ID=tu.Turno_ID)where vale_id='" + id+"'";

                OracleDataReader dtreader = cmd.ExecuteReader();

                if (dtreader.FieldCount > 0)
                {
                    while (dtreader.Read())
                    {
                        auxUser[0] = dtreader.GetString(0);
                        auxUser[1] = dtreader.GetDateTime(1).ToString("dd-MM-yyyy");
                        auxUser[2] = dtreader.GetString(2).ToString();
                        auxUser[3] = dtreader.GetInt32(3).ToString();
                        auxUser[4] = dtreader.GetInt32(4).ToString();
                        auxUser[5] = dtreader.GetString(5);
                        auxUser[6] = dtreader.GetString(6);
                    }

                }
                dtreader.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception("Error al validar a usuario " + ex.Message);
            }
            conn.Close();
            return auxUser;
        }
        public void Procesar_Vale(int id,string nomExt,DateTime fecha,string prod1,string prod2 ,string prod3 ,string prodM,int valvale,int valm,int valtot,string turnom,string menum,string nom,string validacion)
        {
            string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            try
            {
                OracleCommand cmd = new OracleCommand("REGISTRAR_VALE",conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("VALEP_ID",id);
                cmd.Parameters.Add("EmpExt_Nom", nomExt);
                cmd.Parameters.Add("Fecha", fecha);
                cmd.Parameters.Add("Producto1", prod1);
                cmd.Parameters.Add("Producto2", prod2);
                cmd.Parameters.Add("Producto3", prod3);
                cmd.Parameters.Add("ProductoMisc", prodM);
                cmd.Parameters.Add("ValorVale", valvale);
                cmd.Parameters.Add("ValorMisc", valm);
                cmd.Parameters.Add("ValorFinal", valtot);
                cmd.Parameters.Add("Turno_Nom", turnom);
                cmd.Parameters.Add("Menu_nom", menum);
                cmd.Parameters.Add("User_Nom", nom);
                cmd.Parameters.Add("Valido", validacion);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception("Error al validar a usuario " + ex.Message);
            }
            conn.Close();
        }
        public String[] CobrarVale(string id)
        {
            string[] auxUser = new String[9];
            string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select valido,valorvale,turno_nom,menu_nom,producto1,producto2,producto3,productomisc,valorfinal from VALE_PROCESADO where valep_id='"+id+"' and valido='Si'";
                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.FieldCount > 0)
                {
                    while (dtreader.Read())
                    {
                        auxUser[0] = dtreader.GetString(0);
                        auxUser[1] = dtreader.GetInt32(1).ToString();
                        auxUser[2] = dtreader.GetString(2);
                        auxUser[3] = dtreader.GetString(3);
                        auxUser[4] = dtreader.GetString(4);
                        auxUser[5] = dtreader.GetString(5);
                        auxUser[6] = dtreader.GetString(6);
                        auxUser[7] = dtreader.GetString(7);
                        auxUser[8] = dtreader.GetInt32(8).ToString();
                    }
                }
                dtreader.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception("Error al validar a usuario " + ex.Message);
            }
            conn.Close();
            return auxUser;
        }
        public void actualizarvale(int id)
        {
            string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            try
            {
                OracleCommand cmd = new OracleCommand("ACTUALIZAR_VALE", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("VALEP_ID", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception("Error al validar a usuario " + ex.Message);
            }
            conn.Close();
        }

        public Boolean agregarCargo(String nom, int valor)
        {
            Boolean bolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("AGREGAR_CARGO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("CARGO_ID", nom);
                cmd.Parameters.Add("CARGO_NOMBRE", valor);


                cmd.ExecuteNonQuery();
                bolAux = true;
                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, Al Agregar" + ex.Message);
            }
            return bolAux;
        }

        public String cargoPorId(int id)
        {
            string cargo = "";
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select CARGO_NOMBRE from CARGO where CARGO_ID='" + id + "'";

                OracleDataReader dtreader = cmd.ExecuteReader();

                while (dtreader.Read())
                {

                    cargo = dtreader.GetString(0);
                }

            }
            catch
            {
                throw new Exception("Error, No se Encuentra Registro");
            }

            return cargo;
        }

        public Boolean modificarCargo(int id, String nom, int valor)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("MODIFICAR_CARGO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("CARGO_ID", id);
                cmd.Parameters.Add("CARGO_NOMBRE", nom);
                cmd.Parameters.Add("CARGO_VALOR", valor);

                boolAux = true;
                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, Al Modificar" + ex.Message);
            }
            return boolAux;

        }

        public Boolean eliminarCargo(int id)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("ELIMINAR_CARGO", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("CARGO_ID", id);
                cmd.ExecuteNonQuery();
                boolAux = true;
                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            return boolAux;
        }

        public String listarCargo()
        {
            List<Cargo> listAux = new List<Cargo>();

            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select CARGO_ID, CARGO_NOMBRE, CARGO_VALOR FROM CARGO";

                conn.Open();

                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {
                        Cargo cargo = new Cargo
                        {


                            _cargoId = Convert.ToInt32(dtreader[0]),
                            _cargoNom = dtreader[1].ToString(),
                            _cargovalor = Convert.ToInt32(dtreader[2])

                        };
                        listAux.Add(cargo);

                    }
                }


                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            var json = JsonConvert.SerializeObject(listAux);
            return json;
        }

        public Boolean agregarEmpExt(String nom)
        {
            Boolean bolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("AGREGAR_EMP_EXT", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("EMPRSAEXT_NOMBRE", nom);



                cmd.ExecuteNonQuery();
                bolAux = true;
                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, Al Agregar" + ex.Message);
            }
            return bolAux;
        }


        public Boolean modificarEmpExt(int id, String nom)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("MODIFICAR_EMP_EXT", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("EMPRSAEXT_ID", id);
                cmd.Parameters.Add("EMPRSAEXT_NOMBRE", nom);


                boolAux = true;
                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, Al Modificar" + ex.Message);
            }
            return boolAux;

        }

        public Boolean eliminarEmpExt(int id)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("ELIMINAR_EMP_EXT", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("EMPRSAEXT_ID", id);
                cmd.ExecuteNonQuery();
                boolAux = true;
                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            return boolAux;
        }

        public String listarEmpExt()
        {
            List<EmpresaExterna> listAux = new List<EmpresaExterna>();

            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select EMPRSAEXT_ID, EMPRSAEXT_NOMBRE FROM EMPRESA_EXTERNA";

                conn.Open();

                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {
                        EmpresaExterna empExt = new EmpresaExterna
                        {


                            _idEmpExt = Convert.ToInt32(dtreader[0]),
                            _nomEmpExt = dtreader[1].ToString(),


                        };
                        listAux.Add(empExt);

                    }
                }


                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            var json = JsonConvert.SerializeObject(listAux);
            return json;
        }

        public Boolean agregarSucursal(String nom, String dir, String tipo_casino)
        {
            Boolean bolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("AGREGAR_SUCURSAL", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("SUCURSAL_NOMBRE", nom);
                cmd.Parameters.Add("SUCURSAL_DIRECCION", dir);
                cmd.Parameters.Add("SUCURSAL_TIPO_CASINO", tipo_casino);
                cmd.ExecuteNonQuery();
                bolAux = true;
                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, Al Agregar" + ex.Message);
            }
            return bolAux;
        }

        public Boolean modificarSucursal(int id, String nom, String dir, String tipo_casino)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("MODIFICAR_SUCURSAL", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("SUCURSAL_ID", id);
                cmd.Parameters.Add("SUCURSAL_NOMBRE", nom);
                cmd.Parameters.Add("SUCURSAL_DIRECCION", dir);
                cmd.Parameters.Add("SUCURSAL_TIPO_CASINO", tipo_casino);

                boolAux = true;
                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, Al Modificar" + ex.Message);
            }
            return boolAux;

        }

        public Boolean eliminarSucursal(int id)
        {
            bool boolAux = false;
            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;
                conn.Open();
                OracleCommand cmd = new OracleCommand("ELIMINAR_SUCURSAL", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("SUCURSAL_ID", id);
                cmd.ExecuteNonQuery();
                boolAux = true;
                conn.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            return boolAux;
        }

        public String listarSucursal()
        {
            List<Sucursal> listAux = new List<Sucursal>();

            try
            {
                string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
                OracleConnection conn = new OracleConnection();
                conn.ConnectionString = strConn;

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select SUCURSAL_ID, SUCURSAL_NOMBRE, SUCURSAL_DIRECCION, SUCURSAL_TIPO_CASINO FROM SUCURSAL";

                conn.Open();

                OracleDataReader dtreader = cmd.ExecuteReader();
                if (dtreader.HasRows)
                {
                    while (dtreader.Read())
                    {
                        Sucursal suc = new Sucursal
                        {


                            _idSucursal = Convert.ToInt32(dtreader[0]),
                            _nomSucursal = dtreader[1].ToString(),
                            _dirSucursal = dtreader[2].ToString(),
                            _tipoCasino = dtreader[3].ToString()


                        };
                        listAux.Add(suc);

                    }
                }


                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error, No se Encuentra Registro" + ex.Message);
            }
            var json = JsonConvert.SerializeObject(listAux);
            return json;
        }

        public int ObtenerValorProducto(string Prod1, string Prod2, string Prod3)
        {
            int ValorFinal = 0;
            if (Prod1 == "Sin Seleccion" || Prod1 == "")
            {
                Prod1 = " ";
            }
             if (Prod2 == "Sin Seleccion" || Prod2 == "")
            {
                Prod2 = " ";
            }
             if (Prod3 == "Sin Seleccion" || Prod3 == "")
            {
                Prod3 = " ";
            }
            string strConn = "DATA SOURCE=localhost:1521/xe;USER ID=SYSTEM; Password=system;";
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = strConn;
            conn.Open();
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select sum(PRODUCTO_VALOR) from PRODUCTOS where PRODUCTO_NOMBRE='" + Prod1 + "'or PRODUCTO_NOMBRE='" + Prod2 + "'or PRODUCTO_NOMBRE='" + Prod3 + "'";

                OracleDataReader dtreader = cmd.ExecuteReader();

                if (dtreader.FieldCount > 0)
                {
                    while (dtreader.Read())
                    {
                        ValorFinal = dtreader.GetInt32(0);
                    }
                }
                dtreader.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new Exception("Error al validar a usuario " + ex.Message);
            }
            conn.Close();
            return ValorFinal;
        }

    }


}
