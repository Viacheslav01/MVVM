using Microsoft.Phone.Controls;
using MVVM_Article.ViewModels;

namespace MVVM_Article
{
	public partial class MainPage
		: PhoneApplicationPage
	{
		public MainPage()
		{
			InitializeComponent();

			DataContext = new MainPageViewModel();
		}
	}
}