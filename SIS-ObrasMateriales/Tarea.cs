using System;
using System.Collections.Generic;

namespace SIS_ObrasMateriales;

public partial class Tarea
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public float PorcentajeAvance { get; set; }

    public int IdObra { get; set; }

    public virtual Obra IdObraNavigation { get; set; } = null!;
}
