using System.ComponentModel.DataAnnotations;

namespace BibliotecaSemana4.Models;

public class Libro
{
    public int Id { get; set; }

    [Display(Name = "Título")]
    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(150, ErrorMessage = "El título admite hasta 150 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Display(Name = "Autor")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un autor.")]
    public int AutorId { get; set; }

    [Display(Name = "Categoría")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una categoría.")]
    public int CategoriaId { get; set; }

    [Display(Name = "Año de publicación")]
    [Range(1, 9999, ErrorMessage = "El año debe estar entre 1 y 9999.")]
    public int AnioPublicacion { get; set; }

    public Autor? Autor { get; set; }

    [Display(Name = "Categoría")]
    public Categoria? Categoria { get; set; }
}
