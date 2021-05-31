namespace Omnia.Pie.Vtm.Framework.ControlExtenders
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;

	public static class MediaElementExtender
	{
		public static object GetSources(DependencyObject obj)
		{
			return (object)obj.GetValue(SourcesProperty);
		}

		public static void SetSources(DependencyObject obj, object value)
		{
			obj.SetValue(SourcesProperty, value);
		}

		public static readonly DependencyProperty SourcesProperty =
			DependencyProperty.RegisterAttached("Sources", typeof(object), typeof(MediaElementExtender), new PropertyMetadata(null, OnSourcesChanged));

		public static void OnSourcesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var mediaElement = (MediaElement)d;
			mediaElement.MediaEnded -= MediaElement_MediaEnded;
			mediaElement.MediaEnded += MediaElement_MediaEnded;
			mediaElement.Stop();
			mediaElement.Tag = 0; // Next source index.
			PlayNextSource(mediaElement);
		}

		private static void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
		{
			PlayNextSource((MediaElement)sender);
		}

		private static void PlayNextSource(MediaElement mediaElement)
		{
			var index = (int)mediaElement.Tag;
			var sources = GetSources(mediaElement) as IEnumerable<Uri>;

			if (sources == null) return;

			var count = sources.Count();

			if (index >= count)
			{
				index = 0;
			}

			if (index < count)
			{
				mediaElement.Tag = index + 1;
				var source = sources.ElementAt(index);
				mediaElement.Source = source;
				if(count == 1)
					mediaElement.Position = TimeSpan.FromSeconds(0);
				mediaElement.Play();
			}
		}
	}
}