using System;
using System.Windows.Forms;

namespace tpWinForm_EquipoU
{
    public partial class frmAltaMarca : Form
    {
        private Marca marca = null;

        
        public frmAltaMarca()
        {
            InitializeComponent();
        }

        
        public frmAltaMarca(Marca marca)
        {
            InitializeComponent();
            this.marca = marca;

            txtDescripcion.Text = marca.Descripcion;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            MarcaNegocio negocio = new MarcaNegocio();

            try
            {
               
                if (this.marca == null)
                    this.marca = new Marca();

                // Guardamos lo que escribió el usuario
                this.marca.Descripcion = txtDescripcion.Text;

                if (this.marca.Id != 0)
                {
                    negocio.modificar(this.marca);
                    MessageBox.Show("Marca modificada exitosamente.");
                }
                else
                {
                    negocio.agregar(this.marca);
                    MessageBox.Show("Marca agregada exitosamente.");
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}