using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace MVVM_Article
{
	public partial class DetailsPage
		: PhoneApplicationPage
	{
		private bool _isFirstStart = true;

		public DetailsPage()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			
			if(!_isFirstStart)
			{
				return;
			}

			_isFirstStart = false;

			viewProgressPanel.Visibility = Visibility.Visible;

			decimal amount;
			decimal percent;
			int term;

			var value = GetParameter("amount");
			if (!decimal.TryParse(value, out amount))
			{
				MessageBox.Show("Сумма должна быть числом");
				return;
			}

			value = GetParameter("percent");
			if (!decimal.TryParse(value, out percent))
			{
				MessageBox.Show("Процент должен быть числом");
				return;
			}

			value = GetParameter("term");
			if (!int.TryParse(value, out term))
			{
				MessageBox.Show("Срок кредита должен быть числом");
				return;
			}

			viewAmount.Text = amount.ToString("N2");
			viewPercent.Text = percent.ToString("N2");
			viewTerm.Text = term.ToString("D");

			Task.Run(() =>
			{
				try
				{
					var balance = amount;
					var interestRate = percent / 1200;
					var payment = Calculator.CalculatePayment(amount, percent, term);

					var schedule = new List<PaymentRecord>();
					for (var period = 0; period < term; period++)
					{
						var interest = Math.Round(balance * interestRate, 2);
						var loan = payment - interest;
						balance -= loan;

						var record = new PaymentRecord
						{
							Interest = interest,
							Loan = loan,
							Balance = balance
						};

						schedule.Add(record);
					}

					Dispatcher.BeginInvoke(() =>
					{
						viewPayment.Text = payment.ToString("N2");
						viewTotalPayment.Text = (payment * term).ToString("N2");

						var style = (Style)Resources["PhoneTextNormalStyle"];

						foreach(var record in schedule)
						{
							var grid = new Grid();
							grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
							grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
							grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

							var loanElement = new TextBlock
							{
								Text = record.Loan.ToString("N2"),
								Style = style
							};
							Grid.SetColumn(loanElement, 0);

							var interestElement = new TextBlock
							{
								Text = record.Interest.ToString("N2"),
								Style = style
							};
							Grid.SetColumn(interestElement, 1);

							var balanceElement = new TextBlock
							{
								Text = record.Balance.ToString("N2"),
								Style = style
							};
							Grid.SetColumn(balanceElement, 2);

							grid.Children.Add(loanElement);
							grid.Children.Add(interestElement);
							grid.Children.Add(balanceElement);

							viewRecords.Children.Add(grid);
						}
					});
				}
				finally
				{
					Dispatcher.BeginInvoke(() => viewProgressPanel.Visibility = Visibility.Collapsed);
				}
			});
		}

		private string GetParameter(string name)
		{
			string value;

			if (!NavigationContext.QueryString.TryGetValue(name, out value))
			{
				ShowWrongParametersMessage();
			}

			return value;
		}

		private static void ShowWrongParametersMessage()
		{
			MessageBox.Show("ошибка передачи параметров", "ошибка", MessageBoxButton.OK);
		}

		private class PaymentRecord
		{
			public decimal Loan { get; set; }
			public decimal Interest { get; set; }
			public decimal Balance { get; set; }
		}
	}
}