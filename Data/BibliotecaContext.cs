using BibliotecaSemana4.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaSemana4.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
    {
    }

    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Libro> Libros => Set<Libro>();
    public DbSet<Lector> Lectores => Set<Lector>();
    public DbSet<Prestamo> Prestamos => Set<Prestamo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Libro>().HasOne(libro => libro.Autor).WithMany()
            .HasForeignKey(libro => libro.AutorId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Libro>().HasOne(libro => libro.Categoria).WithMany()
            .HasForeignKey(libro => libro.CategoriaId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Prestamo>().HasOne(prestamo => prestamo.Libro).WithMany()
            .HasForeignKey(prestamo => prestamo.LibroId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Prestamo>().HasOne(prestamo => prestamo.Lector).WithMany()
            .HasForeignKey(prestamo => prestamo.LectorId).OnDelete(DeleteBehavior.Restrict);
    }
}
