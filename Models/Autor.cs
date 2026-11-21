using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BibliotecaSemana4.Models;

public class Autor : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(80, ErrorMessage = "El nombre admite hasta 80 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(80, ErrorMessage = "El apellido admite hasta 80 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    [StringLength(60, ErrorMessage = "La nacionalidad admite hasta 60 caracteres.")]
    public string Nacionalidad { get; set; } = string.Empty;

    [Display(Name = "Fecha de nacimiento")]
    [BindRequired, DataType(DataType.Date), Column(TypeName = "date")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime FechaNacimiento { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaNacimiento.Year < 1000 || FechaNacimiento.Date > DateTime.Today)
        {
            yield return new ValidationResult("Ingresa una fecha de nacimiento válida.", [nameof(FechaNacimiento)]);
        }
    }
}
