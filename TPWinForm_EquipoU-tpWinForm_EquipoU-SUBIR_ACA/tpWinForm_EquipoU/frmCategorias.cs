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
    public partial class frmCategorias : Form
    {
        public frmCategorias()
        {
            InitializeComponent();
        }

        private void frmCategorias_Load(object sender, EventArgs e)
        {
            cargarGrid();
        }

        private void cargarGrid()
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            try
            {
                dgvCategorias.DataSource = negocio.listar();
                
                if (dgvCategorias.Columns["Id"] != null)
                    dgvCategorias.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.ToString());
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaCategoria alta = new frmAltaCategoria();
            alta.ShowDialog();
            cargarGrid();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.CurrentRow != null)
            {
                Categoria seleccionado = (Categoria)dgvCategorias.CurrentRow.DataBoundItem;
                frmAltaCategoria modificar = new frmAltaCategoria(seleccionado);
                modificar.ShowDialog();
                cargarGrid();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una categoría de la lista para modificar.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            try
            {
                if (dgvCategorias.CurrentRow != null)
                {
                  
                        Categoria seleccionado = (Categoria)dgvCategorias.CurrentRow.DataBoundItem;
                        negocio.eliminar(seleccionado.Id);
                        cargarGrid();
                    
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione una categoría para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.ToString());
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
