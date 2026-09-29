using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Infrastructure.Persistence.Converters;

namespace MyPlanner.Infrastructure.Persistence.Configurations
{
    internal class PlannerConfiguration : IEntityTypeConfiguration<Planner>
    {
        public void Configure(EntityTypeBuilder<Planner> builder)
        {
            builder.ToTable("Planners");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .HasConversion<PlannerIdConverter>();

            builder.Property(p => p.OwnerId)
                   .HasConversion<UserIdConverter>()
                   .IsRequired();

            builder.Property(p => p.Title)
                   .HasConversion<PlannerTitleConverter>()
                   .HasMaxLength(PlannerTitle.MaxLength)
                   .IsRequired();

            builder.Navigation(p => p.Todos)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany(p => p.Todos, todosBuilder =>
            {
                todosBuilder.ToTable("Todos");
                todosBuilder.HasKey(t => t.Id);

                todosBuilder.Property(t => t.Id)
                            .HasConversion<TodoIdConverter>();

                todosBuilder.Property(t => t.Title)
                            .HasConversion<TodoTitleConverter>()
                            .HasMaxLength(TodoTitle.MaxLength)
                            .IsRequired();

                todosBuilder.Property(t => t.Description)
                            .HasConversion<TodoDescriptionConverter>()
                            .HasMaxLength(TodoDescription.MaxLength)
                            .IsRequired();

                todosBuilder.WithOwner()
                            .HasForeignKey("PlannerId");
            });
        }
    }
}
