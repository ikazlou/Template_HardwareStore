﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Template_HardwareStore.Entities.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; } // Primary key

        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Display Order for category must be greater then 0")]
        public int DisplayOrder { get; set; }
    }
}
