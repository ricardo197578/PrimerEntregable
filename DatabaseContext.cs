using System;
using System.Data.SQLite;
using System.IO;

namespace ClubDeportivo
{
    public class DatabaseContext : IDisposable
    {
        private SQLiteConnection _connection;
        private readonly string _databasePath = "ClubDeportivo.db";

        public DatabaseContext()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            bool databaseExists = File.Exists(_databasePath);

            if (!databaseExists)
            {
                SQLiteConnection.CreateFile(_databasePath);
            }

            _connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", _databasePath));
            _connection.Open();

            //SIEMPRE VERIFICA Y CREA TABLAS QUE NO ESIXTEN
            CreateTables();

            //SOLO POR AHORA CREA ADMIN SIEMPRE POR DEFECTO VER SI DEBE SER ASI
            if (!databaseExists)
            {
                
                CrearAdminPorDefecto();
            }
        }

        private void CreateTables()
        {
            using (var command = new SQLiteCommand(_connection))
            {
                // Tabla Personas
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Personas (
                    Dni TEXT PRIMARY KEY,
                    Nombre TEXT NOT NULL,
                    Apellido TEXT NOT NULL,
                    FechaNacimiento TEXT NOT NULL,
                    Direccion TEXT,
                    Telefono TEXT,
                    Email TEXT,
                    TipoPersona TEXT NOT NULL
                )";
                command.ExecuteNonQuery();

                // Tabla Socios
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Socios (
                    Dni TEXT PRIMARY KEY,
                    NroSocio INTEGER NOT NULL UNIQUE,
                    FechaInscripcion TEXT NOT NULL,
                    EstadoActivo INTEGER NOT NULL,
                    FechaVencimientoCuota TEXT,
                    FOREIGN KEY (Dni) REFERENCES Personas(Dni)
                )";
                command.ExecuteNonQuery();


                // Tabla NoSocios (extiende Personas)
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS NoSocios (
                    Dni TEXT PRIMARY KEY,
                    FechaRegistro TEXT NOT NULL,
                    FOREIGN KEY (Dni) REFERENCES Personas(Dni)
                )";
                command.ExecuteNonQuery();

                // Tabla Profesores (extiende Personas)
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Profesores (
                    Dni TEXT PRIMARY KEY,
                    Legajo TEXT NOT NULL,
                    FechaContratacion TEXT NOT NULL,
                    EsTitular INTEGER NOT NULL,
                    FOREIGN KEY (Dni) REFERENCES Personas(Dni)
                )";
                command.ExecuteNonQuery();

                // Tabla Actividades
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Actividades (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT NOT NULL,
                    Descripcion TEXT,
                    Horario TEXT,
                    ProfesorDni TEXT,
                    PrecioNoSocio REAL,
                    ExclusivaSocios INTEGER NOT NULL,
                    FOREIGN KEY (ProfesorDni) REFERENCES Profesores(Dni)
                )";
                command.ExecuteNonQuery();

                // Tabla Cuotas
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Cuotas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    SocioDni TEXT NOT NULL,
                    Monto REAL NOT NULL,
                    FechaPago TEXT NOT NULL,
                    FechaVencimiento TEXT NOT NULL,
                    MetodoPago TEXT NOT NULL,
                    Cuotas INTEGER NOT NULL,
                    Pagada INTEGER NOT NULL,
                    FOREIGN KEY (SocioDni) REFERENCES Socios(Dni)
                )";
                command.ExecuteNonQuery();

                // Tabla Carnets
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Carnets (
                    NroCarnet INTEGER PRIMARY KEY,
                    SocioDni TEXT NOT NULL,
                    FechaEmision TEXT NOT NULL,
                    FechaVencimiento TEXT NOT NULL,
                    FOREIGN KEY (SocioDni) REFERENCES Socios(Dni)
                )";
                command.ExecuteNonQuery();


                // Tabla AptosFisicos
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS AptosFisicos (
                    SocioDni TEXT PRIMARY KEY,
                    FechaEmision TEXT NOT NULL,
                    FechaVencimiento TEXT NOT NULL,
                    Medico TEXT NOT NULL,
                    Observaciones TEXT,
                    FOREIGN KEY (SocioDni) REFERENCES Socios(Dni)
                )";
                command.ExecuteNonQuery();

                // Tabla de relación Socio-Actividad
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS SocioActividad (
                    SocioDni TEXT NOT NULL,
                    ActividadId INTEGER NOT NULL,
                    PRIMARY KEY (SocioDni, ActividadId),
                    FOREIGN KEY (SocioDni) REFERENCES Socios(Dni),
                    FOREIGN KEY (ActividadId) REFERENCES Actividades(Id)
                )";
                command.ExecuteNonQuery();


                // Tabla de relación NoSocio-Actividad
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS NoSocioActividad (
                    NoSocioDni TEXT NOT NULL,
                    ActividadId INTEGER NOT NULL,
                    FechaPago TEXT NOT NULL,
                    MetodoPago TEXT NOT NULL,
                    PRIMARY KEY (NoSocioDni, ActividadId, FechaPago),
                    FOREIGN KEY (NoSocioDni) REFERENCES NoSocios(Dni),
                    FOREIGN KEY (ActividadId) REFERENCES Actividades(Id)
                )";
                command.ExecuteNonQuery();


                // Tabla Administradores
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Administradores (
                    Dni TEXT PRIMARY KEY,
                    Usuario TEXT UNIQUE NOT NULL,
                    Clave TEXT NOT NULL,
                    FOREIGN KEY (Dni) REFERENCES Personas(Dni)
                )";
                command.ExecuteNonQuery();


            }
        }

        private void CrearAdminPorDefecto()
        {
            using (var command = new SQLiteCommand(_connection))
            {
                // Insertar admin por defecto
                command.CommandText = @"
                INSERT INTO Personas (Dni, Nombre, Apellido, FechaNacimiento, TipoPersona) 
                VALUES ('00000000', 'Admin', 'Sistema', '2000-01-01', 'Administrador')";
                command.ExecuteNonQuery();

                command.CommandText = @"
                INSERT INTO Administradores (Dni, Usuario, Clave) 
                VALUES ('00000000', 'admin', '1234')";
                command.ExecuteNonQuery();
            }
        }

        public SQLiteConnection GetConnection()
        {
            return _connection;
        }
        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

       
    }
}