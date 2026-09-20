using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace tpWinForm_EquipoU
{
    internal class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, I.ImagenUrl, A.IdMarca, A.IdCategoria, A.Precio FROM ARTICULOS A LEFT JOIN IMAGENES I ON A.Id = I.IdArticulo");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    int idArticuloActual = (int)datos.Lector["Id"];

                   
                    Articulo aux = null;
                    foreach (Articulo art in lista)
                    {
                        if (art.Id == idArticuloActual)
                        {
                            aux = art;
                            break;
                        }
                    }

                
                    if (aux == null)
                    {
                        aux = new Articulo();
                        aux.Id = idArticuloActual;
                        aux.Codigo = (string)datos.Lector["Codigo"];
                        aux.Nombre = (string)datos.Lector["Nombre"];
                        aux.Descripcion = (string)datos.Lector["Descripcion"];
                        aux.IdMarca = (int)datos.Lector["IdMarca"];
                        aux.IdCategoria = (int)datos.Lector["IdCategoria"];
                        aux.Precio = (decimal)datos.Lector["Precio"];

                        lista.Add(aux);
                    }

                    
                    if (!(datos.Lector["ImagenUrl"] is DBNull) == true)
                    {
                        string urlImg = (string)datos.Lector["ImagenUrl"];

                        if (aux.ImagenesUrl.Contains(urlImg) == false)
                        {
                            aux.ImagenesUrl.Add(urlImg);
                        }
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Articulo> filtrar(string codigo, string nombre, int idMarca, int idCategoria)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, I.ImagenUrl, A.IdMarca, A.IdCategoria, A.Precio FROM ARTICULOS A LEFT JOIN IMAGENES I ON A.Id = I.IdArticulo WHERE 1 = 1";

                if (!string.IsNullOrEmpty(codigo) == true)
                {
                    consulta += " AND A.Codigo LIKE '%" + codigo + "%'";
                }
                if (!string.IsNullOrEmpty(nombre) == true)
                {
                    consulta += " AND A.Nombre LIKE '%" + nombre + "%'";
                }
                if (idMarca != -1)
                {
                    consulta += " AND A.IdMarca = " + idMarca;
                }
                if (idCategoria != -1)
                {
                    consulta += " AND A.IdCategoria = " + idCategoria;
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    int idArticuloActual = (int)datos.Lector["Id"];

                    Articulo aux = null;
                    foreach (Articulo art in lista)
                    {
                        if (art.Id == idArticuloActual)
                        {
                            aux = art;
                            break;
                        }
                    }

                    if (aux == null)
                    {
                        aux = new Articulo();
                        aux.Id = idArticuloActual;
                        aux.Codigo = (string)datos.Lector["Codigo"];
                        aux.Nombre = (string)datos.Lector["Nombre"];
                        aux.Descripcion = (string)datos.Lector["Descripcion"];
                        aux.IdMarca = (int)datos.Lector["IdMarca"];
                        aux.IdCategoria = (int)datos.Lector["IdCategoria"];
                        aux.Precio = (decimal)datos.Lector["Precio"];

                        lista.Add(aux);
                    }

                    if (!(datos.Lector["ImagenUrl"] is DBNull) == true)
                    {
                        string urlImg = (string)datos.Lector["ImagenUrl"];
                        if (aux.ImagenesUrl.Contains(urlImg) == false)
                        {
                            aux.ImagenesUrl.Add(urlImg);
                        }
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // 1. Guardamos el artículo principal
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) VALUES (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @precio)");

                datos.setearParametro("@codigo", nuevo.Codigo);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@descripcion", nuevo.Descripcion);
                datos.setearParametro("@idMarca", nuevo.IdMarca);
                datos.setearParametro("@idCategoria", nuevo.IdCategoria);
                datos.setearParametro("@precio", nuevo.Precio);

                datos.ejecutarAccion();

                // 2. Buscamos el ID del artículo que acabamos de insertar para asociarle las imágenes
                AccesoDatos datosId = new AccesoDatos();
                datosId.setearConsulta("SELECT Max(Id) FROM ARTICULOS");
                datosId.ejecutarLectura();
                int idArticulo = 0;
                if (datosId.Lector.Read() == true)
                {
                    idArticulo = (int)datosId.Lector[0];
                }
                datosId.cerrarConexion();

                // 3. Recorremos la lista de imágenes con un foreach básico y las guardamos
                if (nuevo.ImagenesUrl != null)
                {
                    foreach (string auxImg in nuevo.ImagenesUrl)
                    {
                        AccesoDatos datosImg = new AccesoDatos();
                        datosImg.setearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@idArticulo, @imagenUrl)");
                        datosImg.setearParametro("@idArticulo", idArticulo);
                        datosImg.setearParametro("@imagenUrl", auxImg);
                        datosImg.ejecutarAccion();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void modificar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // 1. Actualizamos los datos principales del artículo
                datos.setearConsulta("UPDATE ARTICULOS SET Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca, IdCategoria = @idCategoria, Precio = @precio WHERE Id = @id");

                datos.setearParametro("@codigo", articulo.Codigo);
                datos.setearParametro("@nombre", articulo.Nombre);
                datos.setearParametro("@descripcion", articulo.Descripcion);
                datos.setearParametro("@idMarca", articulo.IdMarca);
                datos.setearParametro("@idCategoria", articulo.IdCategoria);
                datos.setearParametro("@precio", articulo.Precio);
                datos.setearParametro("@id", articulo.Id);

                datos.ejecutarAccion();

                // 2. Limpiamos las imágenes viejas de la base de datos para este artículo
                AccesoDatos datosBorrar = new AccesoDatos();
                datosBorrar.setearConsulta("DELETE FROM IMAGENES WHERE IdArticulo = @idArticulo");
                datosBorrar.setearParametro("@idArticulo", articulo.Id);
                datosBorrar.ejecutarAccion();

                // 3. Insertamos las imágenes actuales de la lista
                if (articulo.ImagenesUrl != null)
                {
                    foreach (string auxImg in articulo.ImagenesUrl)
                    {
                        AccesoDatos datosImg = new AccesoDatos();
                        datosImg.setearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@idArticulo, @imagenUrl)");
                        datosImg.setearParametro("@idArticulo", articulo.Id);
                        datosImg.setearParametro("@imagenUrl", auxImg);
                        datosImg.ejecutarAccion();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM ARTICULOS WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }

}