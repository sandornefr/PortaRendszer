using System;
using System.Collections.Generic;

namespace PortaRendszer.Models;

public partial class Felhasznalo
{
    public int Id { get; set; }

    public string Nev { get; set; } = null!;

    public string Beosztas { get; set; } = null!;

    public string Felhasznalonev { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Jelszo { get; set; } = null!;

    public virtual ICollection<Belepes> Belepes { get; set; } = new List<Belepes>();

    public virtual ICollection<Osztaly> Osztalyok { get; set; } = new List<Osztaly>();

    public virtual ICollection<TanteremHasznalat> TanteremHasznalats { get; set; } = new List<TanteremHasznalat>();
}
