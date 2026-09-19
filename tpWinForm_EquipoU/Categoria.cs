using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tpWinForm_EquipoU
{
    internal class Categoria
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public override string ToString()
        {
            return Descripcion; // Le decimos que cuando se muestre como texto, imprima la descripción
        }
    }
}
