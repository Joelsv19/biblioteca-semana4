using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BibliotecaSemana4.Models;

public class Prestamo : IValidatableObject
{
    public int Id { get; set; }

    [Display(Name = "Libro")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un libro.")]
    public int LibroId { get; set; }

    [Display(Name = "Lector")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un lector.")]
    public int LectorId { get; set; }

    [Display(Name = "Fecha de préstamo")]
    [BindRequired, DataType(DataType.Date), Column(TypeName = "date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime FechaPrestamo { get; set; }

    [Display(Name = "Fecha de devolución")]
    [DataType(DataType.Date), Column(TypeName = "date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", NullDisplayText = "Pendiente")]
    public DateTime? FechaDevolucion { get; set; }

    public Libro? Libro { get; set; }
    public Lector? Lector { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaPrestamo.Year < 1000)
        {
            yield return new ValidationResult("Ingresa una fecha de préstamo válida.", [nameof(FechaPrestamo)]);
        }

        if (FechaDevolucion is DateTime devolucion && devolucion.Date < FechaPrestamo.Date)
        {
            yield return new ValidationResult(
                "La devolución debe ser el mismo día del préstamo o posterior.", [nameof(FechaDevolucion)]);
        }
    }
}
