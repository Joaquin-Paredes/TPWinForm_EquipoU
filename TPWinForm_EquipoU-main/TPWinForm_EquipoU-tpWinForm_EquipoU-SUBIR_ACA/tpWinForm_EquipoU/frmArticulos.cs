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
        private List<Articulo> listaArticulos;
        private int indiceImagen = 0;
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
                cboMarca.Items.Clear();
                cboCategoria.Items.Clear();

                foreach (Marca item in marcaNegocio.listar())
                {
                    cboMarca.Items.Add(item);
                }

   
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

          
                dgvArticulos.Columns["Id"].Visible = false;
                dgvArticulos.Columns["IdMarca"].Visible = false;
                dgvArticulos.Columns["IdCategoria"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void cargarImagen(string imagen)
        {
            try
            {
               
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                
                pbxArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                
                indiceImagen = 0;

               
                if (seleccionado.ImagenesUrl != null && (seleccionado.ImagenesUrl.Count > 0) == true)
                {
                    cargarImagen(seleccionado.ImagenesUrl[indiceImagen]);
                }
                else
                {
                   
                    cargarImagen("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
                }
            }
        }

        private void dgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
           
            if (dgvArticulos.CurrentRow != null)
            {
               
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

        
                frmAltaArticulo ventanaModificar = new frmAltaArticulo(seleccionado);
                ventanaModificar.ShowDialog();

                cargarGrid();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un artículo de la lista para modificar.");
            }
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            cargarGrid();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;

            try
            {
               
                if (dgvArticulos.CurrentRow != null)
                {
                    
                    
                        seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                        negocio.eliminar(seleccionado.Id);

                      
                        cargarGrid();
                    
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un artículo de la lista para eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.ToString());
            }
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

               
                frmAltaArticulo ventanaDetalle = new frmAltaArticulo(seleccionado, true);
                ventanaDetalle.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un artículo de la lista para ver el detalle.");
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                if (seleccionado.ImagenesUrl != null && (seleccionado.ImagenesUrl.Count > 0) == true)
                {
                    indiceImagen++; 

                  
                    if ((indiceImagen >= seleccionado.ImagenesUrl.Count) == true)
                    {
                        indiceImagen = 0;
                    }

                    cargarImagen(seleccionado.ImagenesUrl[indiceImagen]);
                }
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

               
                if (seleccionado.ImagenesUrl != null && (seleccionado.ImagenesUrl.Count > 0) == true)
                {
                    indiceImagen--;

                   
                    if ((indiceImagen < 0) == true)
                    {
                        indiceImagen = seleccionado.ImagenesUrl.Count - 1;
                    }

                    cargarImagen(seleccionado.ImagenesUrl[indiceImagen]);
                }
            }
        }
    }
    
}
