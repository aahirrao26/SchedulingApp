﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class MedicalSchedulerDBEntities : DbContext
{
    public MedicalSchedulerDBEntities()
        : base("name=MedicalSchedulerDBEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    public virtual DbSet<UserDetails> UserDetails { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Slots> Slots { get; set; }

    public virtual DbSet<Condition> Conditions { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Type> Types { get; set; }

}

}

