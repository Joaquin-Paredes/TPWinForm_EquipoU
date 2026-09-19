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
    public partial class frmMarcas : Form
    {
        public frmMarcas()
        {
            InitializeComponent();
        }

        private void lblDescripcion_Click(object sender, EventArgs e)
        {

        }

        
        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                dgvMarcas.DataSource = null;
                return;
            }

            MarcaNegocio negocio = new MarcaNegocio();
            try
            {
                dgvMarcas.DataSource = negocio.buscarPorDescripcionArticulo(txtDescripcion.Text.Trim());

                dgvMarcas.Columns["Id"].Visible = false;
                dgvMarcas.Columns["Descripcion"].HeaderText = "Marca";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
