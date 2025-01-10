using System;
using System.Collections.Generic;

namespace WEB_Obras.Models;

public partial class Contrato
{
    public int Id { get; set; }

    public string NumContrato { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public bool Vigencia { get; set; }

    public int IdPersona { get; set; }

    public int IdObra { get; set; }

    public virtual Obra IdObraNavigation { get; set; } = null!;

    public virtual Persona IdPersonaNavigation { get; set; } = null!;
}
