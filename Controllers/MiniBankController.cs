using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Transactions;
using System.Security.Principal;
using Microsoft.SqlServer.Server;
using Utils;
using Microsoft.EntityFrameworkCore;
using MiniBank.Data;
using MiniBank.Models;

namespace MiniBank.Controllers
{
    [ApiController]
    public class MiniBankController : Controller
    {
        private MainContext mainDb;
		Dictionary<string, SubContext> databases;

		public MiniBankController()
        {
            var mainContextOptions = new DbContextOptionsBuilder<MainContext>()
                .UseSqlServer(
                    @" Server=maindb,1433; Database=Main; User Id=sa; Password=Passw0rd;"
                )
                .Options;

            var subContextOptions1 = new DbContextOptionsBuilder<SubContext>()
                .UseSqlServer(
                    @" Server=sub_db1,1433; Database=Rzeszow; User Id=sa; Password=Passw0rd;"
                )
                .Options;

            var subContextOptions2 = new DbContextOptionsBuilder<SubContext>()
                .UseSqlServer(
                    @" Server=sub_db2,1433; Database=Krakow; User Id=sa; Password=Passw0rd;"
                )
                .Options;


            mainDb = new MainContext(mainContextOptions);

			databases = new Dictionary<string, SubContext>();
			databases["Rzeszów"] = new SubContext(subContextOptions1);
			databases["Kraków"] = new SubContext(subContextOptions2);

        }

		[Route("api/accounts/transfer")]
		[HttpPost]
		public object TransferMoney([FromBody] Withdraw withdraw)
		{
            if (withdraw.value <= 0)
				return StatusCode(500);

			// using(var oTran = new TransactionScope())
			// {
			try
            {

                var from = (from account in mainDb.Accounts
                        join department in mainDb.Departments
                        on account.department_id equals department.id
                        where account.id == withdraw.fromId
                        select new { account, department }
                        ).FirstOrDefault();

                var to = (from account in mainDb.Accounts
                        join department in mainDb.Departments
                        on account.department_id equals department.id
                        where account.id == withdraw.toId
                        select new { account=account, department=department }
                        ).FirstOrDefault();



                from.account.balance -= withdraw.value;
                to.account.balance += withdraw.value;
                mainDb.SaveChanges();

                var fromTransaction = new Transactions
                {
                    fromAccount = withdraw.fromId,
                    fromDepartment = from.department.name,
                    toAccount = withdraw.toId,
                    toDepartment = to.department.name,
                    value = -withdraw.value,
                    dateOfTransaction = DateTime.Now
                };

                var toTransaction = new Transactions
                {
                    fromAccount = withdraw.fromId,
                    fromDepartment = from.department.name,
                    toAccount = withdraw.toId,
                    toDepartment = to.department.name,
                    value = withdraw.value,
                    dateOfTransaction = DateTime.Now
                };

                databases[from.department.name].Transactions.Add(fromTransaction);
                databases[from.department.name].SaveChanges();
                databases[to.department.name].Transactions.Add(toTransaction);
                databases[to.department.name].SaveChanges();
            }
            catch 
            {
				return StatusCode(500);
			}
			// 	oTran.Complete();
			// }

			return new { message = "Operation successful" };
		}

		[Route("api/accounts")]
		[HttpGet]
		public IEnumerable<Accounts> GetAllAccounts() => mainDb.Accounts;

		[Route("api/accounts")]
        [HttpPost]
        public Accounts PostAccount([FromBody] Accounts account)
        {
			mainDb.Accounts.Add(account);
			mainDb.SaveChanges();

			return account;
		}

        
		[Route("api/accounts/detailed")]
		[HttpGet]
		public IEnumerable<object> GetAccountsDetailed()
		{
            var rzeszowCustomers = databases["Rzeszów"].Customers.ToList();
            var krakowCustomers = databases["Kraków"].Customers.ToList();
			var accounts = mainDb.Accounts.ToList();
			var rzeszow = mainDb.Departments.Where(d => d.name == "Rzeszów").FirstOrDefault();
			var krakow = mainDb.Departments.Where(d => d.name == "Kraków").FirstOrDefault();

			var detailed = from account in accounts 
				       join customerRz in rzeszowCustomers on account.customer_id equals customerRz.id
                       where account.department_id == rzeszow.id
				       select new {
                           account_id=account.id,
                           department_id=account.department_id, 
                           customer_id=account.customer_id, 
                           balance=account.balance, 
                           fname=customerRz.fname,
                           lname=customerRz.lname,
                           pesel=customerRz.pesel,
                           dateOfBirth=customerRz.dateOfBirth,
                           phone=customerRz.phone,
                           email=customerRz.email
                           };

			var detailed2 = from account in accounts 
				       join customerK in krakowCustomers on account.customer_id equals customerK.id
                       where account.department_id == krakow.id 
				       select new {
                           account_id=account.id,
                           department_id=account.department_id, 
                           customer_id=account.customer_id, 
                           balance=account.balance, 
                           fname=customerK.fname,
                           lname=customerK.lname,
                           pesel=customerK.pesel,
                           dateOfBirth=customerK.dateOfBirth,
                           phone=customerK.phone,
                           email=customerK.email
                           };


			return detailed.Concat(detailed2);
		}
        
		[Route("api/departments")]
        [HttpGet]
		public IEnumerable<Departments> GetAllDepartments() => mainDb.Departments;

		[Route("api/rzeszow/customers")]
        [HttpGet]
		public IEnumerable<Customers> 
        GetRzeszowCustomers() => databases["Rzeszów"].Customers;

		[Route("api/rzeszow/customers")]
        [HttpPost]
        public Customers PostRzeszowCustomer([FromBody] Customers customer)
        {
			databases["Rzeszów"].Customers.Add(customer);
			databases["Rzeszów"].SaveChanges();

			return customer;
		}

		[Route("api/rzeszow/transactions")]
        [HttpGet]
		public IEnumerable<Transactions> 
        GetRzeszowTransactions() => databases["Rzeszów"].Transactions;


		[Route("api/krakow/customers")]
        [HttpGet]
		public IEnumerable<Customers> 
        GetKrakowCustomers() => databases["Kraków"].Customers;

		[Route("api/krakow/customers")]
        [HttpPost]
        public Customers PostKrakowCustomer([FromBody] Customers customer)
        {
			databases["Kraków"].Customers.Add(customer);
			databases["Kraków"].SaveChanges();

			return customer;
		}

		[Route("api/krakow/transactions")]
        [HttpGet]
		public IEnumerable<Transactions> 
        GetKrakowTransactions() => databases["Kraków"].Transactions;

	}
}

