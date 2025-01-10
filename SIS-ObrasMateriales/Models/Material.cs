using System;
using System.Collections.Generic;

namespace SIS_ObrasMateriales;

public partial class Material
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int CantidadTotal { get; set; }

    public virtual ICollection<Movimiento> Movimientos { get; } = new List<Movimiento>();
}
