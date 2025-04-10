using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortaRendszer.Models;

public partial class PortaUzenet
{
    public int Id { get; set; }

    [ForeignKey("Tanulo")]
    public int TanuloId { get; set; }

    public required string Uzenet { get; set; }

    [Required]
    public StatuszTipus Statusz { get; set; }

    public DateTime Idopont { get; set; } = DateTime.Now;

    public Tanulo? Tanulo { get; set; }
}

public enum StatuszTipus
{
    Jelen_van,
    Hianyzik,
    Kulon_foglalkozas,
    Hazament
}       
