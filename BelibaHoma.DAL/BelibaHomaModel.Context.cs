﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BelibaHoma.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BelibaHomaDBEntities : DbContext
    {
        public BelibaHomaDBEntities()
            : base("name=BelibaHomaDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AcademicInstitution> AcademicInstitution { get; set; }
        public virtual DbSet<AcademicMajor> AcademicMajor { get; set; }
        public virtual DbSet<Alert> Alert { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<JobOffer> JobOffer { get; set; }
        public virtual DbSet<PredictionTraining> PredictionTraining { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Trainee> Trainee { get; set; }
        public virtual DbSet<Tutor> Tutor { get; set; }
        public virtual DbSet<TutorReport> TutorReport { get; set; }
        public virtual DbSet<TutorSession> TutorSession { get; set; }
        public virtual DbSet<TutorTrainee> TutorTrainee { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
