using System.ComponentModel.DataAnnotations;

namespace BibliotecaSemana4.Models;

public class Lector
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(80, ErrorMessage = "El nombre admite hasta 80 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(80, ErrorMessage = "El apellido admite hasta 80 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
    [StringLength(150, ErrorMessage = "El correo admite hasta 150 caracteres.")]
    public string Correo { get; set; } = string.Empty;

    [Display(Name = "Teléfono")]
    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "Ingresa un teléfono válido.")]
    [StringLength(25, ErrorMessage = "El teléfono admite hasta 25 caracteres.")]
    public string Telefono { get; set; } = string.Empty;
}
