using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVVM_Article
{
	internal static class Calculator
	{
		public static decimal CalculatePayment(decimal amount, decimal percent, int term)
		{
			Task.Delay(1000).Wait();

			percent /= 1200;
			var common = (decimal) Math.Pow((double) (1 + percent), term);
			var multiplier = percent*common/(common - 1);

			var payment = amount*multiplier;
			return payment;
		}

		public static List<PaymentsScheduleRecord> GetPaymentsSchedule(decimal amount, decimal percent, int term)
		{
			var balance = amount;
			var interestRate = percent / 1200;

			var payment = CalculatePayment(amount, percent, term);

			var schedule = new List<PaymentsScheduleRecord>();
			for (var period = 0; period < term; period++)
			{
				var interest = Math.Round(balance * interestRate, 2);
				var loan = payment - interest;
				balance -= loan;

				var record = new PaymentsScheduleRecord
				{
					Interest = interest,
					Loan = loan,
					Balance = balance
				};

				schedule.Add(record);
			}
			return schedule;
		}
	}
}