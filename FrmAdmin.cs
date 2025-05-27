using System;
using System.Windows.Forms;
using System.Drawing;

namespace ClubDeportivo
{

public class FrmAdmin : Form
{
    private SistemaClub sistema;
    private Button btnListarSocios;
        private Button btnListarNoSocios;
        private Button btnRegistrarSocio;
        private Button btnCerrarSesion;
        

        public FrmAdmin(SistemaClub sistemaClub)
    {
        this.sistema = sistemaClub;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Panel de Administrador";
        this.Size = new Size(350, 250);
        this.StartPosition = FormStartPosition.CenterScreen;

        btnListarSocios = new Button();
        btnListarSocios.Text = "Listar Socios";
            btnListarSocios.Size = new Size(130, 30);
        btnListarSocios.Location = new Point(100, 30);
        //btnListarSocios.Click += (s, e) => new FrmListarSocios(sistema).ShowDialog();
  
        this.Controls.Add(btnListarSocios);

            btnListarNoSocios = new Button();
            btnListarNoSocios.Text = "Listar No Socios";
            btnListarNoSocios.Size = new Size(130, 30);
            btnListarNoSocios.Location = new Point(100, 70);
            //btnListarNoSocios.Click += (s, e) => new FrmListarNoSocios(sistema).ShowDialog();
            this.Controls.Add(btnListarNoSocios);

            btnRegistrarSocio = new Button();
        btnRegistrarSocio.Text = "Registrar Socio";
             btnRegistrarSocio.Size = new Size(130, 30);
            btnRegistrarSocio.Location = new Point(100, 110);
        btnRegistrarSocio.Click += (s, e) => new FrmRegistroSocio(sistema).ShowDialog();
        this.Controls.Add(btnRegistrarSocio);

          
           
            btnCerrarSesion = new Button();
            btnCerrarSesion.Text = "Cerrar sesión";
            btnCerrarSesion.Size = new Size(130, 30);
            btnCerrarSesion.Location = new Point(100, 150);
            btnCerrarSesion.Click += (sender, e) =>
            {
                DialogResult confirm = MessageBox.Show("¿Estás seguro de que querés cerrar sesión?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    Application.Restart(); // Reinicia toda la aplicación
                }
            };
            this.Controls.Add(btnCerrarSesion);





        }
    }
}