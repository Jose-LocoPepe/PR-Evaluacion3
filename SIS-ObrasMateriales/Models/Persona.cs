using System;
using System.Collections.Generic;

namespace SIS_ObrasMateriales;

public partial class Persona
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Rut { get; set; } = null!;

    public virtual ICollection<Contrato> Contratos { get; } = new List<Contrato>();
}
