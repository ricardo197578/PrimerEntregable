using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ClubDeportivo
{
    public class SistemaClub : IDisposable
    {
        private readonly DatabaseContext _dbContext;

        //constructor
        public SistemaClub()
        {
            _dbContext = new DatabaseContext();
            Socios = new List<Socio>();
            CargarSocios();
        }

        //
        public List<Socio> Socios { get; set; }//guardar socios en memoria para manipular

        public bool ValidarAdmin(string usuario, string clave)
        {
            using (var command = new SQLiteCommand(_dbContext.GetConnection()))
            {
                command.CommandText = @"
                SELECT COUNT(*) 
                FROM Administradores 
                WHERE Usuario = @usuario AND Clave = @clave";

                command.Parameters.AddWithValue("@usuario", usuario);
                command.Parameters.AddWithValue("@clave", clave);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private void CargarSocios()
        {
            using (var command = new SQLiteCommand(_dbContext.GetConnection()))
            {
                command.CommandText = @"
                SELECT p.*, s.NroSocio, s.FechaInscripcion, s.EstadoActivo, s.FechaVencimientoCuota
                FROM Personas p
                JOIN Socios s ON p.Dni = s.Dni
                WHERE p.TipoPersona = 'Socio'";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var socio = new Socio(
                            reader["Nombre"].ToString(),
                            reader["Apellido"].ToString(),
                            reader["Dni"].ToString(),
                            DateTime.Parse(reader["FechaNacimiento"].ToString()))
                        {
                            NroSocio = Convert.ToInt32(reader["NroSocio"]),
                            FechaInscripcion = DateTime.Parse(reader["FechaInscripcion"].ToString()),
                            EstadoActivo = Convert.ToBoolean(reader["EstadoActivo"]),
                            FechaVencimientoCuota = reader["FechaVencimientoCuota"] != DBNull.Value ?
                                DateTime.Parse(reader["FechaVencimientoCuota"].ToString()) : (DateTime?)null,
                            Direccion = reader["Direccion"] != DBNull.Value ? reader["Direccion"].ToString() : null,
                            Telefono = reader["Telefono"] != DBNull.Value ? reader["Telefono"].ToString() : null,
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null

                        };

                        Socios.Add(socio);
                    }
                }
            }
        }

        public void RegistrarSocio(Socio socio)
        {
            if (socio == null) throw new ArgumentNullException("El socio no puede ser nulo");

            using (var transaction = _dbContext.GetConnection().BeginTransaction())
            {
                try
                {
                    // Insertar en Personas
                    using (var command = new SQLiteCommand(_dbContext.GetConnection()))
                    {
                        command.CommandText = @"
                        INSERT INTO Personas (Dni, Nombre, Apellido, FechaNacimiento, Direccion, Telefono, Email, TipoPersona)
                        VALUES (@Dni, @Nombre, @Apellido, @FechaNacimiento, @Direccion, @Telefono, @Email, 'Socio')";

                        command.Parameters.AddWithValue("@Dni", socio.Dni);
                        command.Parameters.AddWithValue("@Nombre", socio.Nombre);
                        command.Parameters.AddWithValue("@Apellido", socio.Apellido);
                        command.Parameters.AddWithValue("@FechaNacimiento", socio.FechaNacimiento.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Direccion", socio.Direccion ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Telefono", socio.Telefono ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Email", socio.Email ?? (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }

                    // Insertar en Socios
                    using (var command = new SQLiteCommand(_dbContext.GetConnection()))
                    {
                        command.CommandText = @"
                        INSERT INTO Socios (Dni, NroSocio, FechaInscripcion, EstadoActivo, FechaVencimientoCuota)
                        VALUES (@Dni, @NroSocio, @FechaInscripcion, @EstadoActivo, @FechaVencimientoCuota)";

                        command.Parameters.AddWithValue("@Dni", socio.Dni);
                        command.Parameters.AddWithValue("@NroSocio", socio.NroSocio);
                        command.Parameters.AddWithValue("@FechaInscripcion", socio.FechaInscripcion.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@EstadoActivo", socio.EstadoActivo ? 1 : 0);
                        if (socio.FechaVencimientoCuota.HasValue)
                            command.Parameters.AddWithValue("@FechaVencimientoCuota", socio.FechaVencimientoCuota.Value.ToString("yyyy-MM-dd"));
                        else
                            command.Parameters.AddWithValue("@FechaVencimientoCuota", DBNull.Value);


                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    Socios.Add(socio);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al registrar socio: " + ex.Message);
                }
            }
        }
       
        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }

        /*PARA LA DOCUMENTQACION
        En .NET, la recolección de basura (Garbage Collector) limpia automáticamente los objetos en memoria. Pero no puede liberar recursos externos como:

Conexiones a base de datos (SQLiteConnection, SqlConnection)

Archivos abiertos (FileStream)

Objetos del sistema operativo (ventanas, sockets, etc.)

Entonces usamos Dispose() para cerrar manualmente esos recursos antes de que el objeto desaparezca.
        */



    }
}

