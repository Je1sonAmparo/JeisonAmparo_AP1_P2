using System.ComponentModel.DataAnnotations;

namespace JeisonAmparo_AP1_P2.Models;

public class Ventas
{
    [Key]
    public int VentaId { get; set; }

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Este Campo Es Requerido")]
    public string Observacion { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor que cero")]
    public double Total { get; set; }
}
