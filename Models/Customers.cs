using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MiniBank.Models
{
    public class Customers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id {get;set;}
        public string fname {get;set;}
        public string lname {get;set;}
        public string pesel {get;set;}
        public DateTime dateOfBirth {get;set;}
        public string phone {get;set;}
        public string email {get;set;}
    }
}