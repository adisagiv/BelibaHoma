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
    
    public partial class TutorTrainee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TutorTrainee()
        {
            this.TutorReport = new HashSet<TutorReport>();
        }
    
        public int TutorId { get; set; }
        public int TraineeId { get; set; }
        public int Status { get; set; }
        public int Id { get; set; }
    
        public virtual Trainee Trainee { get; set; }
        public virtual Tutor Tutor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TutorReport> TutorReport { get; set; }
    }
}
