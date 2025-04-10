using PortaRendszer.Models;

namespace PortaRendszer.DTOs
{
    public class PortaUzenetCreateDto
    {
        public int TanuloId { get; set; }
        public string? Uzenet { get; set; }
        public StatuszTipus Statusz { get; set; }
    }
}