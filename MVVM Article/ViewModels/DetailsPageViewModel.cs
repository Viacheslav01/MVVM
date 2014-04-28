using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_Article.ViewModels
{
	public class DetailsPageViewModel
		: INotifyPropertyChanged
	{
		public void Initialize(string amount, string percent, string term)
		{
			Amount = amount;
			Percent = percent;
			Term = term;

			Calculate();
		}

		private string _amount;
		public string Amount
		{
			get { return _amount; }
			set
			{
				_amount = value;
				OnPropertyChanged();
			}
		}

		private string _percent;
		public string Percent
		{
			get { return _percent; }
			set
			{
				_percent = value;
				OnPropertyChanged();
			}
		}

		private string _term;
		public string Term
		{
			get { return _term; }
			set
			{
				_term = value;
				OnPropertyChanged();
			}
		}

		private string _payment;
		public string Payment
		{
			get { return _payment; }
			private set
			{
				_payment = value;
				OnPropertyChanged();
			}
		}

		private string _totalAmount;
		public string TotalAmount
		{
			get { return _totalAmount; }
			private set
			{
				_totalAmount = value;
				OnPropertyChanged();
			}
		}

		private IEnumerable<PaymentsScheduleRecord> _paymentsSchedule;
		public IEnumerable<PaymentsScheduleRecord> PaymentsSchedule
		{
			get { return _paymentsSchedule; }
			private set
			{
				_paymentsSchedule = value;
				OnPropertyChanged();
			}
		}

		private bool _isCalculating;
		public bool IsCalculating
		{
			get { return _isCalculating; }
			set
			{
				_isCalculating = value;
				OnPropertyChanged();
			}
		}

		private void Calculate()
		{
			decimal calculatedAmount;
			decimal calculatedPercent;
			int calculatedTerm;

			if (!decimal.TryParse(Amount, out calculatedAmount))
			{
				MessageBox.Show("Сумма должна быть числом");
				return;
			}

			if (!decimal.TryParse(Percent, out calculatedPercent))
			{
				MessageBox.Show("Процент должен быть числом");
				return;
			}

			if (!int.TryParse(Term, out calculatedTerm))
			{
				MessageBox.Show("Срок кредита должен быть числом");
				return;
			}

			IsCalculating = true;

			Task.Run(() =>
			{
				try
				{
					var payment = Calculator.CalculatePayment(calculatedAmount, calculatedPercent, calculatedTerm);
					var schedule = Calculator.GetPaymentsSchedule(calculatedAmount, calculatedPercent, calculatedTerm);

					App.RootFrame.Dispatcher.BeginInvoke(() =>
					{
						Payment = payment.ToString("N2");
						TotalAmount = (payment * calculatedTerm).ToString("N2");
						PaymentsSchedule = schedule;
					});
				}
				finally
				{
					App.RootFrame.Dispatcher.BeginInvoke(() =>
					{
						IsCalculating = false;
					});
				}
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
