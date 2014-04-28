using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_Article.ViewModels
{
	public class MainPageViewModel
		: INotifyPropertyChanged
	{
		private decimal _calculatedAmount;
		private decimal _calculatedPercent;
		private int _calculatedTerm;

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

		private bool _isCalculated;
		public bool IsCalculated
		{
			get { return _isCalculated; }
			set
			{
				_isCalculated = value;
				OnPropertyChanged();
				DetailsCommand.RiseCanExecuteChanged();
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
				DetailsCommand.RiseCanExecuteChanged();
			}
		}

		private DelegateCommand _calculateCommand;
		public DelegateCommand CalculateCommand
		{
			get
			{
				if(_calculateCommand == null)
				{
					_calculateCommand = new DelegateCommand(o => Calculate());
				}

				return _calculateCommand;
			}
		}

		private DelegateCommand _detailsCommand;
		public DelegateCommand DetailsCommand
		{
			get
			{
				if(_detailsCommand == null)
				{
					_detailsCommand = new DelegateCommand(o => GotToDetailsPage(), o => IsCalculated);
				}

				return _detailsCommand;
			}
		}

		private void GotToDetailsPage()
		{
			var pageUri = string.Format("/DetailsPage.xaml?amount={0}&percent={1}&term={2}", _calculatedAmount, _calculatedPercent, _calculatedTerm);
			App.RootFrame.Navigate(new Uri(pageUri, UriKind.Relative));
		}

		private void Calculate()
		{
			IsCalculated = false;
			
			if (!decimal.TryParse(Amount, out _calculatedAmount))
			{
				MessageBox.Show("Сумма должна быть числом");
				return;
			}

			if (!decimal.TryParse(Percent, out _calculatedPercent))
			{
				MessageBox.Show("Процент должен быть числом");
				return;
			}

			if (!int.TryParse(Term, out _calculatedTerm))
			{
				MessageBox.Show("Срок кредита должен быть числом");
				return;
			}

			IsCalculating = true;

			Task.Run(() =>
			{
				try
				{
					var payment = Calculator.CalculatePayment(_calculatedAmount, _calculatedPercent, _calculatedTerm);

					App.RootFrame.Dispatcher.BeginInvoke(() =>
					{
						IsCalculated = true;
						Payment = payment.ToString("N2");
						TotalAmount = (payment * _calculatedTerm).ToString("N2");
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
