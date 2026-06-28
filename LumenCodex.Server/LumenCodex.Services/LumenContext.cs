using Microsoft.EntityFrameworkCore;
using LumenCodex.Domain.Entities;

namespace LumenCodex.Services;

public class LumenContext : DbContext
{
    public LumenContext(DbContextOptions<LumenContext> options) : base(options) 
    {}
    
    public DbSet<Course> Courses { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Subtitle> Subtitles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>().HasKey(c => c.CourseId);
        modelBuilder.Entity<Section>().HasKey(s => s.SectionId);
        modelBuilder.Entity<Lesson>().HasKey(l => l.LessonId);
        modelBuilder.Entity<Subtitle>().HasKey(s => s.SubtitleId);
        modelBuilder.Entity<Note>().HasKey(n => n.NoteId);
        
        // Course-Section
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Sections)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        // Section-Lesson
        modelBuilder.Entity<Section>()
            .HasMany(s => s.Lessons)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Section>()
            .HasOne<Course>()
            .WithMany(c => c.Sections)
            .HasForeignKey(s => s.CourseId);
        
        modelBuilder.Entity<Lesson>()
            .HasOne<Section>()
            .WithMany(s => s.Lessons)
            .HasForeignKey(l => l.SectionId);
        
        // Lesson-Subtitle
        modelBuilder.Entity<Lesson>()
            .HasMany(l => l.Subtitles)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Subtitle>()
            .HasOne<Lesson>()
            .WithMany(l => l.Subtitles)
            .HasForeignKey(s => s.LessonId);
        
        // Note-Lesson
        modelBuilder.Entity<Lesson>()
            .HasMany(l => l.Notes)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Note>()
            .HasOne<Lesson>()
            .WithMany(l => l.Notes)
            .HasForeignKey(n => n.LessonId);
    }
}