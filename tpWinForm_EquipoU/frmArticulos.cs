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
    public partial class frmArticulos : Form
    {
        public frmArticulos()
        {
            InitializeComponent();
        }

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            cargarGrid();

            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                // Recorremos la lista y agregamos cada marca a mano
                foreach (Marca item in marcaNegocio.listar())
                {
                    cboMarca.Items.Add(item);
                }

                // Hacemos lo mismo con las categorías
                foreach (Categoria item in categoriaNegocio.listar())
                {
                    cboCategoria.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cargarGrid()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                dgvArticulos.DataSource = negocio.listar();

                // Ocultamos las columnas de IDs para que quede limpio
                dgvArticulos.Columns["Id"].Visible = false;
                dgvArticulos.Columns["IdMarca"].Visible = false;
                dgvArticulos.Columns["IdCategoria"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                try
                {
                    if (!string.IsNullOrEmpty(seleccionado.ImagenUrl))
                    {
                        pbxArticulo.Load(seleccionado.ImagenUrl);
                    }
                    else
                    {
                        pbxArticulo.Image = null;
                    }
                }
                catch (Exception)
                {
                    pbxArticulo.Image = null;
                }
            }
        }

        private void dgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboMarca_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {

                int idMarca = -1;
                if (cboMarca.SelectedItem != null)
                {
                    Marca marcaSel = (Marca)cboMarca.SelectedItem;
                    idMarca = marcaSel.Id;
                }


                int idCategoria = -1;
                if (cboCategoria.SelectedItem != null)
                {
                    Categoria categoriaSel = (Categoria)cboCategoria.SelectedItem;
                    idCategoria = categoriaSel.Id;
                }


                dgvArticulos.DataSource = negocio.filtrar(txtCodigo.Text, txtNombre.Text, idMarca, idCategoria);


                dgvArticulos.Columns["Id"].Visible = false;
                dgvArticulos.Columns["IdMarca"].Visible = false;
                dgvArticulos.Columns["IdCategoria"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
