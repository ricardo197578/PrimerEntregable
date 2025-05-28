
using System;
using System.Windows.Forms;
using System.Data.SQLite;
using ClubDeportivo;

namespace ClubDeportivo
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            // Configuración inicial para Windows Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            
            // try-catch para mostrar cualquier error al usuario.

            try
            {
                // Verificar/Crear admin por defecto
                using (DatabaseContext db = new DatabaseContext())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(db.GetConnection()))
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM Administradores";
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            cmd.CommandText = @"
                                INSERT INTO Personas (Dni, Nombre, Apellido, FechaNacimiento, TipoPersona) 
                                VALUES ('00000000', 'Admin', 'Sistema', '2000-01-01', 'Administrador')";
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = @"
                                INSERT INTO Administradores (Dni, Usuario, Clave) 
                                VALUES ('00000000', 'admin', '1234')";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // Iniciar el formulario de login con manejo de recursos al llamar a SistemaClub
                using (SistemaClub sistema = new SistemaClub())
                {
                    Application.Run(new FrmLogin(sistema));
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Error al iniciar la aplicación:\n" + ex.Message,
                "Error crítico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            }
        }
    }
}

