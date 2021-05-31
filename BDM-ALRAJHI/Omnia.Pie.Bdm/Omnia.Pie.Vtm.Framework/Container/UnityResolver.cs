namespace Omnia.Pie.Vtm.Framework.Container
{
    using Microsoft.Practices.Unity;
    using Omnia.Pie.Vtm.Framework.Interface;
    using System;

    /// <summary>
    /// https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/src/IOC/XLabs.IOC.Unity/UnityResolver.cs
    /// </summary>
    public class UnityResolver : IResolver
	{
		private IUnityContainer Container { get; }

		public UnityResolver(IUnityContainer container)
		{
			Container = container ?? throw new ArgumentNullException(nameof(container));
		}

		public T Resolve<T>()
		{
			return Container.Resolve<T>();
		}

		public T Resolve<T>(params ResolverOverride[] param)
		{
			return Container.Resolve<T>(param);
		}

		public object Resolve(Type type)
		{
			return Container.Resolve(type);
		}

		public bool IsRegistered(Type type)
		{
			return Container.IsRegistered(type);
		}
	}
}
