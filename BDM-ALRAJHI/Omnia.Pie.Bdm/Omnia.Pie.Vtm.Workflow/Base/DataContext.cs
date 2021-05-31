namespace Omnia.Pie.Vtm.Workflow
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public class DataContext<TContext> : IDataContext where TContext : new()
	{
		private readonly TContext _context;
		private readonly Dictionary<Type, object> _cache;

		public DataContext() : this(new TContext())
		{

		}

		public DataContext(TContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			_context = context;
			_cache = new Dictionary<Type, object>();
		}

		public T Get<T>() where T : class
		{
			T t;
			if (!TryGet(out t))
			{
				throw new InvalidOperationException($"Data context {GetType()} doesn't support context interface {typeof(T)}.");
			}

			return t;
		}

		public bool TryGet<T>(out T t) where T : class
		{
			object cache;
			if (_cache.TryGetValue(typeof(T), out cache))
			{
				t = (T)cache;
			}
			else
			{
				t = FindContext<T>();

				if (t == null)
				{
					t = _context as T;
				}

				_cache.Add(typeof(T), t);
			}

			return (t != null);
		}

		private T FindContext<T>() where T : class
		{
			var properties = _context.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (var p in properties)
			{
				if (typeof(T).IsAssignableFrom(p.PropertyType))
				{
					object pValue = p.GetValue(_context);
					return (T)pValue;
				}

				if (typeof(T).IsSubclassOf(p.PropertyType))
				{
					object pValue = p.GetValue(_context);
					T t = pValue as T;
					if (t != null)
					{
						return t;
					}
				}
			}

			return null;
		}
	}
}