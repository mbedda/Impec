using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImpecMC.Models
{
    public class Division
    {
        public int Id { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
    }
}