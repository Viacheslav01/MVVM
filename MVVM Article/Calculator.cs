using System;
using System.Threading.Tasks;

namespace MVVM_Article
{
	public class Calculator
	{
		public static decimal CalculatePayment(decimal amount, decimal percent, int term)
		{
			Task.Delay(3000).Wait();

			percent /= 1200;
			var common = (decimal) Math.Pow((double) (1 + percent), term);
			var multiplier = percent*common/(common - 1);

			var payment = amount*multiplier;
			return payment;
		}
	}
}