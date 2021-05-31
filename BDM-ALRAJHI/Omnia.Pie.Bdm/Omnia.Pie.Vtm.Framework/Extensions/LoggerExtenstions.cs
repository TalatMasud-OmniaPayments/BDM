namespace Omnia.Pie.Vtm.Framework.Extensions
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.CompilerServices;

	public static class LoggerExtenstions
	{
		public static void Properties(this ILogger logger, object x, string[] properties = null, [CallerMemberName] string caller = null)
		{
			var propertyInfos = x.GetType().GetProperties().OfType<PropertyInfo>().Where(i => !i.PropertyType.IsArray);
			if (properties != null)
				propertyInfos = propertyInfos.Join(properties, i => i.Name, i => i, (i0, i1) => i0);
			logger.Info($".{caller}({x.GetType().Name}):\n\t{string.Join(", ", propertyInfos.Select(i => $"{i.Name} = {i.GetValue(x)}"))}");
		}

		public static void Info2(this ILogger logger, string message, [CallerMemberName] string caller = null) =>
			logger.Info($"{new StackTrace(1, false).GetFrame(0).GetMethod().DeclaringType.Name}.{caller}: {message}");
	}
}