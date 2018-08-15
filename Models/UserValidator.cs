using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CsBeltExam.Models
{
    public class UserValidator
    {
        [Required]
        public string FirstName {get;set;}

        [Required]
        public string LastName {get;set;}

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username {get;set;}

        [Required]
        [MinLength(8)]
        [Compare("ConfirmPassword")]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword {get;set;}
    }
}