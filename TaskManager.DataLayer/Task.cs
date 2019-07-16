namespace TaskManager.DataLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Task
    {
        [Key]
        public int Task_ID { get; set; }

        [Column("Task")]
        [Required]
        [StringLength(100)]
        public string Task1 { get; set; }

        public int? Parent_ID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }

        public bool Status { get; set; }
    }
}
