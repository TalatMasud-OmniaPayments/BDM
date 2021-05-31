namespace ESpaceCommunication.Test.ViewModels
{
	using System;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;

	public abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
	{
		public virtual string Id { get; set; }
		public virtual void Load() { }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(storage, value))
				return false;

			storage = value;
			OnPropertyChanged(propertyName);

			return true;
		}

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public abstract void Dispose();
	}
}