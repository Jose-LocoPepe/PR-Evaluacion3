using System;
using System.Collections.Generic;

namespace WEB_Materiales.Models;

public partial class Movimiento
{
    public int IdMaterial { get; set; }

    public int IdObra { get; set; }

    public int Cantidad { get; set; }

    public virtual Material IdMaterialNavigation { get; set; } = null!;

    public virtual Obra IdObraNavigation { get; set; } = null!;
}
