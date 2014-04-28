using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using MVVM_Article.ViewModels;

namespace MVVM_Article
{
	public partial class DetailsPage
		: PhoneApplicationPage
	{
		private bool _isFirstStart = true;

		public DetailsPage()
		{
			InitializeComponent();

			DataContext = new DetailsPageViewModel();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			
			if(!_isFirstStart)
			{
				return;
			}
			_isFirstStart = false;

			var viewModel = (DetailsPageViewModel)DataContext;
			viewModel.Initialize(GetParameter("amount"), GetParameter("percent"), GetParameter("term"));
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
	}
}