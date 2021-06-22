using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MiniBank.Models
{

	public class Accounts
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id {get;set;}
		public int department_id {get;set;}
		public int customer_id {get;set;}
		public double balance {get;set;}
	}
}