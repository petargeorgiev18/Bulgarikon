using Bulgarikon.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bulgarikon.Data.Configurations;

namespace Bulgarikon.Data
{
    public class BulgarikonDbContext : IdentityDbContext<BulgarikonUser, IdentityRole<Guid>,Guid>
    {
        public BulgarikonDbContext(DbContextOptions<BulgarikonDbContext> options)
        : base(options)
        {
        }
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Civilization> Civilizations { get; set; } = null!;
        public DbSet<Figure> Figures { get; set; } = null!;
        public DbSet<Era> Eras { get; set; } = null!;
        public DbSet<Artifact> Artifacts { get; set; } = null!;
        public DbSet<Quiz> Quizzes { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<QuizResult> QuizResults { get; set; } = null!;
        public DbSet<EventCivilization> EventCivilizations { get; set; } = null!;
        public DbSet<EventFigure> EventFigures { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Artifact>()
                .HasOne(a => a.Era)
                .WithMany(e => e.Artifacts)
                .HasForeignKey(a => a.EraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Figure>()
                .HasOne(f => f.Era)
                .WithMany(e => e.Figures)
                .HasForeignKey(f => f.EraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Event>()
                .HasOne(e => e.Era)
                .WithMany(er => er.Events)
                .HasForeignKey(e => e.EraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EventCivilization>()
                .HasOne(ec => ec.Event)
                .WithMany(e => e.EventCivilizations)
                .HasForeignKey(ec => ec.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EventCivilization>()
                .HasOne(ec => ec.Civilization)
                .WithMany(c => c.EventCivilizations)
                .HasForeignKey(ec => ec.CivilizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EventFigure>()
                .HasOne(ef => ef.Event)
                .WithMany(e => e.EventFigures)
                .HasForeignKey(ef => ef.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EventFigure>()
                .HasOne(ef => ef.Figure)
                .WithMany(f => f.EventFigures)
                .HasForeignKey(ef => ef.FigureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Question>()
                .HasOne(q => q.Quiz)
                .WithMany(z => z.Questions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<QuizResult>()
                .HasOne(r => r.Quiz)
                .WithMany()
                .HasForeignKey(r => r.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<QuizResult>()
                .HasOne(r => r.User)
                .WithMany(u => u.QuizResults)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ApplyConfiguration(new ArtifactConfiguration());
            builder.ApplyConfiguration(new CivilizationConfiguration());
            builder.ApplyConfiguration(new EraConfiguration());
            builder.ApplyConfiguration(new EventConfiguration());
            builder.ApplyConfiguration(new FigureConfiguration());
            builder.ApplyConfiguration(new QuizConfiguration());
            builder.ApplyConfiguration(new QuestionConfiguration());
            builder.ApplyConfiguration(new AnswerConfiguration());
        }
    }
}
