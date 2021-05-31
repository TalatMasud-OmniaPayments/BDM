﻿using System.Threading;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Framework.Extensions
{
	/// <summary>
	/// Credits : https://stackoverflow.com/a/26282531/1106625
	/// </summary>
	public static class TaskExtensions
	{
		public static Task<TResult> OrWhenCancelled<TResult>(this Task<TResult> mainTask, CancellationToken cancellationToken)
		{
			if (!cancellationToken.CanBeCanceled)
				return mainTask;

			return OrWhenCancelled_(mainTask, cancellationToken);
		}

		private static async Task<TResult> OrWhenCancelled_<TResult>(this Task<TResult> mainTask, CancellationToken cancellationToken)
		{
			Task cancellationTask = Task.Delay(Timeout.Infinite, cancellationToken);
			await Task.WhenAny(mainTask, cancellationTask).ConfigureAwait(false);

			cancellationToken.ThrowIfCancellationRequested();
			return await mainTask;
		}

		public static Task OrWhenCancelled(this Task mainTask, CancellationToken cancellationToken)
		{
			if (!cancellationToken.CanBeCanceled)
				return mainTask;

			return OrWhenCancelled_(mainTask, cancellationToken);
		}

		private static async Task OrWhenCancelled_(this Task mainTask, CancellationToken cancellationToken)
		{
			Task cancellationTask = Task.Delay(Timeout.Infinite, cancellationToken);
			await Task.WhenAny(mainTask, cancellationTask).ConfigureAwait(false);
			cancellationToken.ThrowIfCancellationRequested();
			await mainTask;
		}
	}
}
