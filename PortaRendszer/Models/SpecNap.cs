using System;
using System.Collections.Generic;

namespace PortaRendszer.Models;

public partial class SpecNap
{
    public int Id { get; set; }

    public DateOnly Datum { get; set; }

    public string Megnevezes { get; set; } = null!;

    public bool? FelugyeletOpcio { get; set; }
}
