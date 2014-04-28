using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Controls;

namespace MVVM_Article
{
	public partial class MainPage
		: PhoneApplicationPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void CalculateClick(object sender, RoutedEventArgs e)
		{
			decimal amount;
			decimal percent;
			int term;

			if(!decimal.TryParse(viewAmount.Text, out amount))
			{
				viewProgressPanel.Visibility = Visibility.Collapsed;
				MessageBox.Show("Сумма должна быть числом");
				return;
			}

			if(!decimal.TryParse(viewPercent.Text, out percent))
			{
				viewProgressPanel.Visibility = Visibility.Collapsed;
				MessageBox.Show("Процент должен быть числом");
				return;
			}

			if(!int.TryParse(viewTerm.Text, out term))
			{
				viewProgressPanel.Visibility = Visibility.Collapsed;
				MessageBox.Show("Срок кредита должен быть числом");
				return;
			}

			Focus();
			viewProgressPanel.Visibility = Visibility.Visible;

			Task.Run(() =>
				{
					try
					{
						var payment = Calculator.CalculatePayment(amount, percent, term);

						Dispatcher.BeginInvoke(() =>
							{
								viewCalculationPanel.Visibility = Visibility.Visible;
								viewPayment.Text = payment.ToString("N2");
								viewTotalPayment.Text = (payment * term).ToString("N2");
							});
					}
					finally
					{
						Dispatcher.BeginInvoke(() =>
							{
								viewProgressPanel.Visibility = Visibility.Collapsed;
							});
					}
				});
		}

		private void DetailsClick(object sender, RoutedEventArgs e)
		{
			var pageUri = string.Format("/DetailsPage.xaml?amount={0}&percent={1}&term={2}", viewAmount.Text, viewPercent.Text, viewTerm.Text);
			NavigationService.Navigate(new Uri(pageUri, UriKind.Relative));
		}
	}
}