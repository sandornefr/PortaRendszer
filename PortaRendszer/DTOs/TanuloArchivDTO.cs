using PortaRendszer.Models;

public class TanuloArchivDTO
{
    public int Id { get; set; }
    public string Nev { get; set; } = string.Empty;
    public string OsztalyNev { get; set; } = string.Empty;
    public DateTime TorlesDatum { get; set; }

    public static TanuloArchivDTO FromEntity(TanuloArchiv a) => new()
    {
        Id = a.Id,
        Nev = a.Nev,
        OsztalyNev = a.OsztalyNev,
        TorlesDatum = a.TorlesIdopont
    };
}
