using System;
using System.Collections.Generic;

namespace SIS_ObrasMateriales;

public partial class Obra
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public float AvanceGeneral { get; set; }

    public virtual ICollection<Contrato> Contratos { get; } = new List<Contrato>();

    public virtual ICollection<Tarea> Tareas { get; } = new List<Tarea>();
}
