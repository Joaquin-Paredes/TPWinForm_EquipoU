using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace tpWinForm_EquipoU
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;

        public frmAltaArticulo()
        {
            InitializeComponent();
            cargarCombos();
        }

        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            cargarCombos();

            txtCodigo.Text = articulo.Codigo;
            txtNombre.Text = articulo.Nombre;
            txtDescripcion.Text = articulo.Descripcion;
            txtPrecio.Text = articulo.Precio.ToString();
            txtImagenUrl.Text = string.Join(", ", articulo.ImagenesUrl);

            foreach (Marca item in cboMarca.Items)
            {
                if (item.Id == articulo.IdMarca)
                {
                    cboMarca.SelectedItem = item;
                    break;
                }
            }

            foreach (Categoria item in cboCategoria.Items)
            {
                if (item.Id == articulo.IdCategoria)
                {
                    cboCategoria.SelectedItem = item;
                    break;
                }
            }
        }

        public frmAltaArticulo(Articulo articulo, bool soloLectura)
        {
            InitializeComponent();
            this.articulo = articulo;
            cargarCombos();

            txtCodigo.Text = articulo.Codigo;
            txtNombre.Text = articulo.Nombre;
            txtDescripcion.Text = articulo.Descripcion;
            txtPrecio.Text = articulo.Precio.ToString();
            txtImagenUrl.Text = string.Join(", ", articulo.ImagenesUrl); // CORREGIDO AQUÍ

            if (soloLectura == true)
            {
                txtCodigo.Enabled = false;
                txtNombre.Enabled = false;
                txtDescripcion.Enabled = false;
                txtPrecio.Enabled = false;
                txtImagenUrl.Enabled = false;

                foreach (Marca item in cboMarca.Items)
                {
                    if (item.Id == articulo.IdMarca)
                    {
                        cboMarca.SelectedItem = item;
                        break;
                    }
                }

                foreach (Categoria item in cboCategoria.Items)
                {
                    if (item.Id == articulo.IdCategoria)
                    {
                        cboCategoria.SelectedItem = item;
                        break;
                    }
                }
                cboMarca.Enabled = false;
                cboCategoria.Enabled = false;

                btnAceptar.Visible = false;
                btnCancelar.Text = "Cerrar";
            }
        }

        private void cargarCombos()
        {
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
                MessageBox.Show("Ocurrió un error al ir a buscar los datos: " + ex.ToString());
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (cboMarca.SelectedItem == null || cboCategoria.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione una marca y una categoría de la lista.");
                return;
            }

            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (this.articulo == null)
                    this.articulo = new Articulo();

                this.articulo.Codigo = txtCodigo.Text;
                this.articulo.Nombre = txtNombre.Text;
                this.articulo.Descripcion = txtDescripcion.Text;
                this.articulo.Precio = decimal.Parse(txtPrecio.Text);

                Marca marcaSeleccionada = (Marca)cboMarca.SelectedItem;
                this.articulo.IdMarca = marcaSeleccionada.Id;

                Categoria categoriaSeleccionada = (Categoria)cboCategoria.SelectedItem;
                this.articulo.IdCategoria = categoriaSeleccionada.Id;

                // CORREGIDO AQUÍ: Leemos el TextBox y rellenamos la lista de imágenes separándolas por comas
                this.articulo.ImagenesUrl.Clear();
                if (!string.IsNullOrWhiteSpace(txtImagenUrl.Text))
                {
                    string[] vectorUrls = txtImagenUrl.Text.Split(',');
                    foreach (string url in vectorUrls)
                    {
                        string urlLimpia = url.Trim();
                        if (!string.IsNullOrEmpty(urlLimpia))
                        {
                            this.articulo.ImagenesUrl.Add(urlLimpia);
                        }
                    }
                }

                if (this.articulo.Id != 0)
                {
                    negocio.modificar(this.articulo);
                    MessageBox.Show("Modificado exitosamente.");
                }
                else
                {
                    negocio.agregar(this.articulo);
                    MessageBox.Show("Agregado exitosamente.");
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