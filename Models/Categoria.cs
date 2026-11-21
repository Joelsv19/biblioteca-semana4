using System.ComponentModel.DataAnnotations;

namespace BibliotecaSemana4.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(80, ErrorMessage = "El nombre admite hasta 80 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(250, ErrorMessage = "La descripción admite hasta 250 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;

    [Display(Name = "Código")]
    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(20, ErrorMessage = "El código admite hasta 20 caracteres.")]
    public string Codigo { get; set; } = string.Empty;

    public bool Activa { get; set; }
}
