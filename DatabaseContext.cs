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

            //_connection = new SQLiteConnection($"Data Source={_databasePath};Version=3;");
            //_connection.Open();
            _connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", _databasePath));
            _connection.Open();

            if (!databaseExists)
            {
                CreateTables();
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