using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JeisonAmparo_AP1_P2.Models;

public class VentasDetalle
{
    [Key]
    public int DetalleId { get; set; }

    public int VentaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido")]
    public int ProductoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
    public int Cantidad { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
    public double Precio { get; set; }

    [ForeignKey("VentaId")]
    [InverseProperty("Detalles")]
    public virtual Ventas Venta { get; set; } = null!;

    [ForeignKey("ProductoId")]
    [InverseProperty("VentasDetalle")]
    public virtual Productos Producto { get; set; } = null!;
}
