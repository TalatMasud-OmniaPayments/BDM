namespace Omnia.Pie.Vtm.Framework.Interface
{
	using Microsoft.Practices.Unity;
	using System;

	public interface IResolver
	{
		T Resolve<T>();
		T Resolve<T>(params ResolverOverride[] param);
		object Resolve(Type type);
	}
}