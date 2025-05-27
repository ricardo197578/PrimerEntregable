using System;
using System.Windows.Forms;
using System.Drawing;

namespace ClubDeportivo
{
    public class FrmLogin : Form
    {
        private SistemaClub sistema;
        private TextBox txtUsuario;
        private TextBox txtClave;
        private Button btnIngresar;
        private ComboBox cmbRol;

        public FrmLogin(SistemaClub sistemaClub)
        {
            this.sistema = sistemaClub;
            Console.WriteLine("FrmLogin ha sido instanciado correctamente.");
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Login - Club Deportivo";
            this.Size = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterScreen;


            // ComboBox para selección de tipo de usuario
            Label lblRol = new Label();
            lblRol.Text = "Rol:";
            lblRol.Location = new Point(20, 20);
            this.Controls.Add(lblRol);

            cmbRol = new ComboBox();
            cmbRol.Location = new Point(120, 20);
            cmbRol.Width = 150;
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Items.Add("Administrador");
            cmbRol.Items.Add("Socio");
            cmbRol.SelectedIndex = 0; // Por defecto: Administrador
            cmbRol.SelectedIndexChanged += (s, e) =>
            {
                txtClave.Enabled = true;
                //txtClave.Enabled = cmbRol.SelectedItem.ToString() == "Administrador";
            };
            this.Controls.Add(cmbRol);

            // Usuario
            Label lblUsuario = new Label();
            lblUsuario.Text = "Usuario/N° Socio:";
            lblUsuario.Location = new Point(20, 60);
            this.Controls.Add(lblUsuario);

            txtUsuario = new TextBox();
            txtUsuario.Location = new Point(120, 60);
            txtUsuario.Width = 150;
            this.Controls.Add(txtUsuario);

            // Clave
            Label lblClave = new Label();
            lblClave.Text = "Clave:";
            lblClave.Location = new Point(20, 90);
            this.Controls.Add(lblClave);

            txtClave = new TextBox();
            txtClave.Location = new Point(120, 90);
            txtClave.Width = 150;
            txtClave.PasswordChar = '*';
            txtClave.Enabled = true; // Para admin por defecto
            this.Controls.Add(txtClave);

            // Botón Ingresar
            btnIngresar = new Button();
            btnIngresar.Text = "Ingresar";
            btnIngresar.Location = new Point(120, 130);
            btnIngresar.Click += new EventHandler(btnIngresar_Click);
            this.Controls.Add(btnIngresar);
        }

        
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string rolSeleccionado = cmbRol.SelectedItem.ToString();

                if (rolSeleccionado == "Administrador")
                {
                    if (sistema.ValidarAdmin(txtUsuario.Text.Trim(), txtClave.Text.Trim()))
                    {
                        MessageBox.Show("Acceso concedido como Administrador");
                        this.Hide();
                        new FrmAdmin(sistema).Show();
                    }
                    else
                    {
                        MessageBox.Show("Credenciales incorrectas.");
                    }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en el inicio de sesión: " + ex.Message);
            }
        }

    }
}
