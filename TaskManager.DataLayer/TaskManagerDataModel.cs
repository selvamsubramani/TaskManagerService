namespace TaskManager.DataLayer
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TaskManagerDataModel : DbContext
    {
        public TaskManagerDataModel()
            : base("name=TaskManagerDataModel")
        {
        }

        public virtual DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.ChildTask)
                .WithOptional(e => e.ParentTask)
                .HasForeignKey(e => e.ParentID);
        }
    }
}
