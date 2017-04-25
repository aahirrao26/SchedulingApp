﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointmentScheduler.Core.Data
{
    [MetadataType(typeof(TypeMetadata))]
    public partial class Type
    {

    }

    public class TypeMetadata
    {
        public int ID { get; set; }

        public String Name { get; set; }
    }
}
