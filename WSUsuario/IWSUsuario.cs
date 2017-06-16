using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WSUsuario
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IWSUsuario
    {
        [OperationContract]
        Boolean validarLogin(String id, string pass);
        [OperationContract]
        String obtenerUsuario(String id);
        [OperationContract]
        String listarUsuario();
        [OperationContract]
        void agregarUsuario(String id, String nomCom, String pass, String correo, int saldo, String tipo, int cargo_id, string vale_ext);
        [OperationContract]
        Boolean eliminarUsuario(String id);
        [OperationContract]
        Boolean modificarUsuario(String id, String nomCom, String pass, String correo, int saldo, String tipo, int cargo_id, string vale_ext);
        [OperationContract]
        String ObtenerTipo(string id, string pass);
        [OperationContract]
        String[] ObtenerValeTurno(string id);
        [OperationContract]
        List<String> ObtenerProducto(string NomMenu);
        [OperationContract]
        string[] ObtenerMenu(string idTurno);
        [OperationContract]
        String listarTurnos();
        [OperationContract]
        String listarMenu();
        [OperationContract]
        String listarProductos();
        [OperationContract]
        Boolean modificarTurno(int id, String nomProd, int masterTurnoCol);
        [OperationContract]
        Boolean modificarMenu(int id, String nomMenu, int turnoId, int p1_id, int p2_id, int p3_id, int p4_id, int n_p1_id, int n_p2_id, int n_p3_id, int n_p4_id);
        [OperationContract]
        Boolean modificarProds(int id, String nomProd, int valorProd);
        [OperationContract]
        Boolean eliminarTurno(int id);
        [OperationContract]
        Boolean eliminarMenu(int id);
        [OperationContract]
        Boolean eliminarProd(int id);
        [OperationContract]
        void agregarProducto(int id, String nomProd, int valor);
        [OperationContract]
        bool agregarMenu(int id, String nomMenu, int turnoId, int p1_id, int p2_id, int p3_id, int p4_id);
        [OperationContract]
        Producto ProdsPorId(int id);
        [OperationContract]
        List<DetalleMenu> DetalleporId(int id);
        [OperationContract]
        String listTablaMenu();
        [OperationContract]
        Boolean agregarCargo(String nom, int valor);
        [OperationContract]
        Boolean modificarCargo(int id, String nom, int valor);
        [OperationContract]
        Boolean eliminarCargo(int id);
        [OperationContract]
        String listarCargo();
        [OperationContract]
        Boolean agregarEmpExt(String nom);
        [OperationContract]
        Boolean modificarEmpExt(int id, String nom);
        [OperationContract]
        Boolean eliminarEmpExt(int id);
        [OperationContract]
        String listarEmpExt();
        [OperationContract]
        Boolean agregarSucursal(String nom, String dir, String tipo_casino);
        [OperationContract]
        Boolean modificarSucursal(int id, String nom, String dir, String tipo_casino);
        [OperationContract]
        Boolean eliminarSucursal(int id);
        [OperationContract]
        String listarSucursal();
        [OperationContract]
        void Procesar_Vale(int id, string nomExt, DateTime fecha, string prod1, string prod2, string prod3, string prodM, int valvale, int valm, int valtot, string turnom, string menum, string nom, string validacion);
        [OperationContract]
        int ObtenerValorProducto(string Prod1, string Prod2, string Prod3);
        [OperationContract]
        string[] ImprimirVale(string id);
        [OperationContract]
        String[] CobrarVale(string id);
        [OperationContract]
        void actualizarvale(int id);
        [OperationContract]
        String cargoPorId(int id);


    }

    [DataContract]
    public class Usuario {
        [DataMember]
        public String _id { get; set; }
        [DataMember]
        public String _nombreCompleto { get; set; }
        [DataMember]
        public String _password { get; set; }
        [DataMember]
        public String _correo { get; set; }
        [DataMember]
        public int _saldo { get; set; }
        [DataMember]
        public String _tipo { get; set; }
        [DataMember]
        public int _cargoId { get; set; }
        [DataMember]
        public String _valeExt { get; set; }





    }
    [DataContract]
    public class Producto
    {
        [DataMember]
        public int _productoId { get; set; }
        [DataMember]
        public String _productoNombre { get; set; }
        [DataMember]
        public int _productoValor { get; set; }



    }
    [DataContract]
    public class Menu
    {
        [DataMember]
        public int _menuId { get; set; }
        [DataMember]
        public String _menuNombre { get; set; }
        [DataMember]
        public int _turnoId { get; set; }

    }
    [DataContract]
    public class Vale
    {
        [DataMember]
        public int _valeId { get; set; }
        [DataMember]
        public DateTime _valeFecha { get; set; }
        [DataMember]
        public int _valorTotalUsuario { get; set; }
        [DataMember]
        public String _valeUserId { get; set; }
        [DataMember]
        public int _empExtId { get; set; }
        [DataMember]
        public String _sucursalId { get; set; }
        [DataMember]
        public int _turnoId { get; set; }

    }
    [DataContract]
    public class Turno
    {
        [DataMember]
        public int _turnoId { get; set; }
        [DataMember]
        public String _turnoNom { get; set; }
        [DataMember]
        public int _masterturnoColacionCod { get; set; }


    }
    [DataContract]
    public class DetalleMenu
    {
        [DataMember]
        public int _idMenu { get; set; }
        [DataMember]
        public int _prodId { get; set; }
    }

    [DataContract]
    public class EmpresaExterna
    {
        [DataMember]
        public int _idEmpExt { get; set; }
        public String _nomEmpExt { get; set; }
    }



    [DataContract]
    public class tablaDetMenu
    {
        [DataMember]
        public int _idMenu { get; set; }
        [DataMember]
        public String _nomMenu { get; set; }
        [DataMember]
        public int _turnoMenu { get; set; }
        [DataMember]
        public String _prod1 { get; set; }
        [DataMember]
        public String _prod2 { get; set; }
        [DataMember]
        public String _prod3 { get; set; }
        [DataMember]
        public String _prod4 { get; set; }
        [DataMember]
        public int _valorProd1 { get; set; }
        [DataMember]
        public int _valorProd2 { get; set; }
        [DataMember]
        public int _valorProd3 { get; set; }
        [DataMember]
        public int _valorProd4 { get; set; }
        [DataMember]
        public int _totalValor { get; set; }
    }
    [DataContract]
    public class Cargo
    {
        [DataMember]
        public int _cargoId { get; set; }
        [DataMember]
        public String _cargoNom { get; set; }
        [DataMember]
        public int _cargovalor { get; set; }

    }
    [DataContract]
    public class Sucursal
    {
        [DataMember]
        public int _idSucursal { get; set; }
        [DataMember]
        public String _nomSucursal { get; set; }
        [DataMember]
        public String _dirSucursal { get; set; }
        [DataMember]
        public String _tipoCasino { get; set; }

    }
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
