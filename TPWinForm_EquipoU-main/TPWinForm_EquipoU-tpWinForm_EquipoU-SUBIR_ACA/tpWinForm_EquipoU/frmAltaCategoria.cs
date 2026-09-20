using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tpWinForm_EquipoU
{
    public partial class frmAltaCategoria : Form
    {
        private Categoria categoria = null;

      
        public frmAltaCategoria()
        {
            InitializeComponent();
        }

        public frmAltaCategoria(Categoria categoria)
        {
            InitializeComponent();
            this.categoria = categoria;
            txtDescripcion.Text = categoria.Descripcion;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            try
            {
                if (this.categoria == null)
                    this.categoria = new Categoria();

                this.categoria.Descripcion = txtDescripcion.Text;

                if (this.categoria.Id != 0)
                {
                    negocio.modificar(this.categoria);
                    MessageBox.Show("Categoría modificada exitosamente.");
                }
                else
                {
                    negocio.agregar(this.categoria);
                    MessageBox.Show("Categoría agregada exitosamente.");
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

        private void lblDescripcion_Click(object sender, EventArgs e)
        {

        }
    }
}
