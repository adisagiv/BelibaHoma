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
    
    public partial class User
    {
        public int Id { get; set; }
        public string IdNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public System.DateTime CreationTime { get; set; }
        public int UserRole { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> Area { get; set; }
    
        public virtual Trainee Trainee { get; set; }
        public virtual Tutor Tutor { get; set; }
    }
}
