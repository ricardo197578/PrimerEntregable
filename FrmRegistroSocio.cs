using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data.SQLite;

namespace ClubDeportivo
{
    public class FrmRegistroSocio : Form
    {
        private SistemaClub sistema;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtDni;
        private DateTimePicker dtpFechaNacimiento;
        private TextBox txtDireccion;
        private TextBox txtTelefono;
        private TextBox txtEmail;
        private TextBox txtNumeroSocio;
        private Button btnRegistrar;
        private Button btnCancelar;

        public FrmRegistroSocio(SistemaClub sistemaClub)
        {
            this.sistema = sistemaClub;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            
            this.Text = "Registro de Socio";
            this.Size = new Size(450, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int y = 20, xLabel = 20, xText = 150, spacing = 30;

            // Nombre
            Label lblNombre = new Label()
            {
                Text = "Nombre:",
                Location = new Point(xLabel, y),
                Width = 120
            };
            this.Controls.Add(lblNombre);
            txtNombre = new TextBox()
            {
                Location = new Point(xText, y),
                Width = 250
            };
            this.Controls.Add(txtNombre);
            y += spacing;

            // Apellido
            Label lblApellido = new Label()
            {
                Text = "Apellido:",
                Location = new Point(xLabel, y)
            };
            this.Controls.Add(lblApellido);
            txtApellido = new TextBox()
            {
                Location = new Point(xText, y),
                Width = 250
            };
            this.Controls.Add(txtApellido);
            y += spacing;

            // DNI
            Label lblDni = new Label() { Text = "DNI:", Location = new Point(xLabel, y) };
            this.Controls.Add(lblDni);
            txtDni = new TextBox() {Location = new Point(xText, y),Width = 250};
            this.Controls.Add(txtDni);
            y += spacing;

            // Número de Socio
            Label lblNumeroSocio = new Label() { Text = "Número Socio:", Location = new Point(xLabel, y) };
            this.Controls.Add(lblNumeroSocio);
            txtNumeroSocio = new TextBox() { Location = new Point(xText, y), Width = 150 };
            this.Controls.Add(txtNumeroSocio);
            y += spacing;

           
            // Fecha de Nacimiento
            Label lblFechaNacimiento = new Label()
            {
                Text = "Fecha Nacimiento:",
                Location = new Point(xLabel, y)
            };
            this.Controls.Add(lblFechaNacimiento);
            dtpFechaNacimiento = new DateTimePicker()
            {
                Location = new Point(xText, y),
                Width = 250,
                Format = DateTimePickerFormat.Short
            };
            this.Controls.Add(dtpFechaNacimiento);
            y += spacing;

            // Dirección
            Label lblDireccion = new Label()
            {
                Text = "Dirección:",
                Location = new Point(xLabel, y)
            };
            this.Controls.Add(lblDireccion);
            txtDireccion = new TextBox()
            {
                Location = new Point(xText, y),
                Width = 250
            };
            this.Controls.Add(txtDireccion);
            y += spacing;

            // Teléfono
            Label lblTelefono = new Label()
            {
                Text = "Teléfono:",
                Location = new Point(xLabel, y)
            };
            this.Controls.Add(lblTelefono);
            txtTelefono = new TextBox()
            {
                Location = new Point(xText, y),
                Width = 250
            };
            this.Controls.Add(txtTelefono);
            y += spacing;

            // Email
            Label lblEmail = new Label()
            {
                Text = "Email:",
                Location = new Point(xLabel, y)
            };
            this.Controls.Add(lblEmail);
            txtEmail = new TextBox()
            {
                Location = new Point(xText, y),
                Width = 250
            };
            this.Controls.Add(txtEmail);
            y += spacing;


            // Botones
            btnRegistrar = new Button()
            {
                Text = "Registrar",
                Location = new Point(xText, y),
                Width = 100,
                DialogResult = DialogResult.OK
            };
            btnRegistrar.Click += new EventHandler(btnRegistrar_Click);
            this.Controls.Add(btnRegistrar);

            
        }

            // En el evento btnRegistrar_Click:
            private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(txtDni.Text) ||
                    string.IsNullOrWhiteSpace(txtNumeroSocio.Text))
                {
                    MessageBox.Show("Complete todos los campos obligatorios");
                    return;
                }

                var nuevoSocio = new Socio(
                    txtNombre.Text.Trim(),
                    txtApellido.Text.Trim(),
                    txtDni.Text.Trim(),
                    dtpFechaNacimiento.Value)
                {
                    Direccion = txtDireccion.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    NroSocio = Convert.ToInt32(txtNumeroSocio.Text)
                };

                sistema.RegistrarSocio(nuevoSocio);
                MessageBox.Show("Socio registrado exitosamente");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
