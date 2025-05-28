using System;
using System.Windows.Forms;
using System.Drawing;

namespace ClubDeportivo
{

public class FrmAdmin : Form
{
    private SistemaClub sistema;
    private Button btnListarSociosCuotaVencida;
        private Button btnRegistrarSocio;
        private Button btnRegistrarNoSocio;//nuevo
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

        btnListarSociosCuotaVencida = new Button();
        btnListarSociosCuotaVencida.Text = "Vencimiento de Cuotas";
        btnListarSociosCuotaVencida.Size = new Size(130, 30);
        btnListarSociosCuotaVencida.Location = new Point(100, 30);
        //btnListarSocios.Click += (s, e) => new FrmListarSocios(sistema).ShowDialog();
  
        this.Controls.Add(btnListarSociosCuotaVencida);

            

            btnRegistrarSocio = new Button();
        btnRegistrarSocio.Text = "Registrar Socio";
             btnRegistrarSocio.Size = new Size(130, 30);
            btnRegistrarSocio.Location = new Point(100, 70);
        btnRegistrarSocio.Click += (s, e) => new FrmRegistroSocio(sistema).ShowDialog();
        this.Controls.Add(btnRegistrarSocio);

            //nuevo
            btnRegistrarNoSocio = new Button();
            btnRegistrarNoSocio.Text = "Registrar NoSocio";
            btnRegistrarNoSocio.Size = new Size(130, 30);
            btnRegistrarNoSocio.Location = new Point(100, 110);
           // btnRegistrarNoSocio.Click += (s, e) => new FrmRegistroNoSocio(sistema).ShowDialog();
            this.Controls.Add(btnRegistrarNoSocio);



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