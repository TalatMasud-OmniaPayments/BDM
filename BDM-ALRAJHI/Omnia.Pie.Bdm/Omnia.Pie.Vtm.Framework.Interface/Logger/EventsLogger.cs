namespace Omnia.Pie.Vtm.Framework.Interface.Logger
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public class EventsLogger
	{
		private readonly ILogger _logger;
		private readonly List<EventSubscription> _eventsSubscriptions;

		public EventsLogger(ILogger logger, object eventsContainer)
		{
			_logger = logger;
			_eventsSubscriptions = new List<EventSubscription>();
			foreach (EventInfo eventInfo in eventsContainer.GetType().GetEvents())
			{
				_eventsSubscriptions.Add(new EventSubscription(eventsContainer, eventInfo, LogEvent));
			}
		}

		public void StartEventsLogging()
		{
			foreach (EventSubscription eventSubscription in _eventsSubscriptions)
			{
				eventSubscription.SubscribeToEvent();
			}
		}

		public void StopEventsLogging()
		{
			foreach (EventSubscription eventSubscription in _eventsSubscriptions)
			{
				eventSubscription.UnsubscribeFromEvent();
			}
		}

		private void LogEvent(EventSubscription subscription, object sender, object e)
		{
			_logger.Info($"[{this}]: event [{subscription.EventInfo.Name}] occurred: sender=[{sender}], e=[{e}].");
		}

		private class EventSubscription
		{
			public EventSubscription(
				object eventContainer,
				EventInfo eventInfo,
				Action<EventSubscription, object, object> eventProcessor)
			{
				EventContainer = eventContainer;
				EventInfo = eventInfo;
				EventProcessor = eventProcessor;

				EventHandlerMethod = CreateEventHandlerMethod(eventInfo);
			}

			public object EventContainer { get; }
			public EventInfo EventInfo { get; }
			private Action<EventSubscription, object, object> EventProcessor { get; }
			private Delegate EventHandlerMethod { get; }

			public void SubscribeToEvent()
			{
				EventInfo.AddEventHandler(EventContainer, EventHandlerMethod);
			}

			public void UnsubscribeFromEvent()
			{
				EventInfo.RemoveEventHandler(EventContainer, EventHandlerMethod);
			}

			private Delegate CreateEventHandlerMethod(EventInfo eventInfo)
			{
				MethodInfo invokeMethod = eventInfo.EventHandlerType.GetMethod("Invoke");
				if (invokeMethod == null)
				{
					return null;
				}

				var eventParameters = invokeMethod.GetParameters();
				if (eventParameters.Length != 2)
				{
					return null;
				}
				Type senderType = eventParameters[0].ParameterType;
				Type eventArgsType = eventParameters[1].ParameterType;

				MethodInfo genericHandlerMethod = GetType().GetMethod(nameof(EventHandler), BindingFlags.NonPublic | BindingFlags.Static);
				MethodInfo specializedHandlerMethod = genericHandlerMethod.MakeGenericMethod(new[] { senderType, eventArgsType });

				Delegate eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, specializedHandlerMethod);
				return eventHandler;
			}

			private static void EventHandler<TSender, TEventArgs>(EventSubscription subscription, TSender sender, TEventArgs e)
			{
				if (subscription.EventProcessor != null)
				{
					subscription.EventProcessor.Invoke(subscription, sender, e);
				}
			}
		}
	}
}