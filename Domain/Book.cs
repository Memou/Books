using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(256)]
        public string Title { get; set; }
        [Required]
        [StringLength(256)]
        public string Author { get; set; }

        [Required]
        [Range(0.1,1000000)]    
        public decimal Price { get; set; }

        [StringLength(int.MaxValue)]
        public string Description { get; set; }
        [StringLength(int.MaxValue)]
        public string CoverImage { get; set; }

        //probably this class would also have ISBN id but skipping per requirements.  

    }
}
