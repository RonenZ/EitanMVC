using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eitan.Web.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage="*")]
        public string Message { get; set; }
        [Required(ErrorMessage = "*")]
        //[RegularExpression(@"\b[A-Z0-9._%+-]+@(?:[A-Z0-9-]+\.)+[A-Z]{2,4}\b", ErrorMessage = "invalid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
    }
}