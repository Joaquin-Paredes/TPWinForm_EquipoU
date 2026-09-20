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
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.IdMarca = (int)datos.Lector["IdMarca"];
                    aux.IdCategoria = (int)datos.Lector["IdCategoria"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    lista.Add(aux);
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

        if (!string.IsNullOrEmpty(codigo))
        {
            consulta += " AND A.Codigo LIKE '%" + codigo + "%'";
        }
        if (!string.IsNullOrEmpty(nombre))
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
            Articulo aux = new Articulo();
            aux.Id = (int)datos.Lector["Id"];
            aux.Codigo = (string)datos.Lector["Codigo"];
            aux.Nombre = (string)datos.Lector["Nombre"];
            aux.Descripcion = (string)datos.Lector["Descripcion"];
            aux.IdMarca = (int)datos.Lector["IdMarca"];
            aux.IdCategoria = (int)datos.Lector["IdCategoria"];
            aux.Precio = (decimal)datos.Lector["Precio"];

            if (!(datos.Lector["ImagenUrl"] is DBNull))
                aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

            lista.Add(aux);
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
             
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) VALUES (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @precio)");

                datos.setearParametro("@codigo", nuevo.Codigo);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@descripcion", nuevo.Descripcion);
                datos.setearParametro("@idMarca", nuevo.IdMarca);
                datos.setearParametro("@idCategoria", nuevo.IdCategoria);
                datos.setearParametro("@precio", nuevo.Precio);

                datos.ejecutarAccion();

            
                if (!string.IsNullOrWhiteSpace(nuevo.ImagenUrl))
                {
                   
                    AccesoDatos datosId = new AccesoDatos();
                    datosId.setearConsulta("SELECT Max(Id) FROM ARTICULOS");
                    datosId.ejecutarLectura();
                    int idArticulo = 0;
                    if (datosId.Lector.Read())
                    {
                        idArticulo = (int)datosId.Lector[0];
                    }
                    datosId.cerrarConexion();

                  
                    AccesoDatos datosImg = new AccesoDatos();
                    datosImg.setearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@idArticulo, @imagenUrl)");
                    datosImg.setearParametro("@idArticulo", idArticulo);
                    datosImg.setearParametro("@imagenUrl", nuevo.ImagenUrl);
                    datosImg.ejecutarAccion();
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
      
                datos.setearConsulta("UPDATE ARTICULOS SET Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca, IdCategoria = @idCategoria, Precio = @precio WHERE Id = @id");

                datos.setearParametro("@codigo", articulo.Codigo);
                datos.setearParametro("@nombre", articulo.Nombre);
                datos.setearParametro("@descripcion", articulo.Descripcion);
                datos.setearParametro("@idMarca", articulo.IdMarca);
                datos.setearParametro("@idCategoria", articulo.IdCategoria);
                datos.setearParametro("@precio", articulo.Precio);
                datos.setearParametro("@id", articulo.Id);

                datos.ejecutarAccion();

           
                if (!string.IsNullOrWhiteSpace(articulo.ImagenUrl))
                {
                 
                    AccesoDatos datosCheck = new AccesoDatos();
                    datosCheck.setearConsulta("SELECT Id FROM IMAGENES WHERE IdArticulo = @idArticulo");
                    datosCheck.setearParametro("@idArticulo", articulo.Id);
                    datosCheck.ejecutarLectura();

                    if (datosCheck.Lector.Read())
                    {
                        datosCheck.cerrarConexion();
                        AccesoDatos datosUpdate = new AccesoDatos();
                        datosUpdate.setearConsulta("UPDATE IMAGENES SET ImagenUrl = @imagenUrl WHERE IdArticulo = @idArticulo");
                        datosUpdate.setearParametro("@imagenUrl", articulo.ImagenUrl);
                        datosUpdate.setearParametro("@idArticulo", articulo.Id);
                        datosUpdate.ejecutarAccion();
                    }
                    else
                    {
                       
                        datosCheck.cerrarConexion();
                        AccesoDatos datosInsert = new AccesoDatos();
                        datosInsert.setearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@idArticulo, @imagenUrl)");
                        datosInsert.setearParametro("@idArticulo", articulo.Id);
                        datosInsert.setearParametro("@imagenUrl", articulo.ImagenUrl);
                        datosInsert.ejecutarAccion();
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