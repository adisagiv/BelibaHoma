//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Trainee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Trainee()
        {
            this.Grade = new HashSet<Grade>();
            this.TutorTrainee = new HashSet<TutorTrainee>();
        }
    
        public int UserId { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public int AcademicInstitutionId { get; set; }
        public System.DateTime Birthday { get; set; }
        public int AcademicYear { get; set; }
        public int AcademicMajorId { get; set; }
        public Nullable<int> AcademicMinorId { get; set; }
        public string PhoneNumber { get; set; }
        public int MaritalStatus { get; set; }
        public Nullable<int> EmploymentStatus { get; set; }
        public int TutorHoursBonding { get; set; }
        public int TutorHours { get; set; }
        public string NeededHelpDescription { get; set; }
        public int PhysicsLevel { get; set; }
        public int MathLevel { get; set; }
        public int EnglishLevel { get; set; }
        public Nullable<int> AcademicMajorNeededHelpId { get; set; }
        public int SemesterNumber { get; set; }
    
        public virtual AcademicInstitution AcademicInstitution { get; set; }
        public virtual AcademicMajor AcademicMajor { get; set; }
        public virtual AcademicMajor AcademicMajor1 { get; set; }
        public virtual AcademicMajor AcademicMajor2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Grade> Grade { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TutorTrainee> TutorTrainee { get; set; }
        public virtual User User { get; set; }
    }
}
