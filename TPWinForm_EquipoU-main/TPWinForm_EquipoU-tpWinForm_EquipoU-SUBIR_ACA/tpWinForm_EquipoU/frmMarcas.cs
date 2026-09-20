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
        private void frmMarcas_Load(object sender, EventArgs e)
        {
            cargarGrid();
        }

        private void cargarGrid()
        {
            MarcaNegocio negocio = new MarcaNegocio();
            try
            {
                dgvMarcas.DataSource = negocio.listar();

              
                if (dgvMarcas.Columns["Id"] != null)
                    dgvMarcas.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar marcas: " + ex.ToString());
            }
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaMarca alta = new frmAltaMarca();
            alta.ShowDialog();
            cargarGrid();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvMarcas.CurrentRow != null)
            {
                Marca seleccionado = (Marca)dgvMarcas.CurrentRow.DataBoundItem;

                frmAltaMarca modificar = new frmAltaMarca(seleccionado);
                modificar.ShowDialog();
                cargarGrid();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una marca de la lista.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            MarcaNegocio negocio = new MarcaNegocio();
            try
            {
                if (dgvMarcas.CurrentRow != null)
                {
                    Marca seleccionado = (Marca)dgvMarcas.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargarGrid();  
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione una marca para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.ToString());
            }
        }
    }
}
