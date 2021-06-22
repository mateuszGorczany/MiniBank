using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MiniBank.Models
{
    public class Transactions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id {get;set;}
        public int fromAccount {get;set;}
        public string fromDepartment {get;set;}
        public int toAccount {get;set;}
        public string toDepartment {get;set;}
        public double value {get;set;}
        public DateTime dateOfTransaction {get;set;}
    }
}