
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace MedicalAppointmentScheduler.Core.Data
{

using System;
    using System.Collections.Generic;
    
public partial class UserDetails
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public UserDetails()
    {

        this.User_Address = new HashSet<UserAddress>();

        this.User_Login = new HashSet<UserLogin>();

        this.Appointments = new HashSet<Appointment>();

        this.Appointments1 = new HashSet<Appointment>();

        this.Appointments2 = new HashSet<Appointment>();

        this.Conditions = new HashSet<Condition>();

    }


    public int ID { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Phone { get; set; }

    public string EmailAdress { get; set; }

    public Nullable<int> RoleID { get; set; }



    public virtual UserRole L_User_Roles { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UserAddress> User_Address { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<UserLogin> User_Login { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Appointment> Appointments { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Appointment> Appointments1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Appointment> Appointments2 { get; set; }

    public virtual Patient Patient { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Condition> Conditions { get; set; }

}

}
