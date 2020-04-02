using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="SubCategory Name")]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category category { get; set; }
    }
}
