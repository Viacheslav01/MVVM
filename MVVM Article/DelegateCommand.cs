using System;
using System.Windows.Input;

namespace MVVM_Article
{
	public sealed class DelegateCommand
		: ICommand
	{
		private readonly Action<object> _execute;
		private readonly Func<object, bool> _canExecute;

		public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			if(execute == null)
			{
				throw new ArgumentNullException();
			}

			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null
				|| _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			if(!CanExecute(parameter))
			{
				return;
			}

			_execute(parameter);
		}

		public event EventHandler CanExecuteChanged;
		
		public void RiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;
			if(handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}
	}
}
