using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Omnia.Pie.Vtm.Framework.Logger
{
	public static class LoggerContext
	{
		internal static IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

		public static void SetProperty(string propertyName, object value)
		{
			if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));
			if (value == null) throw new ArgumentNullException(nameof(value));

			Properties[propertyName] = value;
		}
	}

	internal static class LoggerContextFormatter
	{
		public static void Format(StringBuilder value)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));

			foreach (var property in LoggerContext.Properties)
			{
				value.Replace($"{{{property.Key}}}", property.Value.ToString());
			}
		}

		public static string Format(string value)
		{
			var stringBuilder = new StringBuilder(value);
			Format(stringBuilder);
			return stringBuilder.ToString(); ;
		}
	}

	public class CustomRollingFlatFileTraceListenerData : TraceListenerData
	{
		private const string FileNamePropertyName = "fileName";
		private const string FooterPropertyName = "footer";
		private const string FormatterPropertyName = "formatter";
		private const string HeaderPropertyName = "header";
		private const string TimeStampPatternPropertyName = "timeStampPattern";
		private const string RollMessagePropertyName = "rollMessage";

		[ConfigurationProperty(FileNamePropertyName, DefaultValue = "rolling.log")]
		public string FileName
		{
			get
			{
				return (string)this[FileNamePropertyName];
			}
			set
			{
				this[FileNamePropertyName] = value;
			}
		}

		[ConfigurationProperty(FooterPropertyName, DefaultValue = "----------------------------------------", IsRequired = false)]
		public string Footer
		{
			get
			{
				return (string)this[FooterPropertyName];
			}
			set
			{
				this[FooterPropertyName] = value;
			}
		}

		[ConfigurationProperty(FormatterPropertyName, IsRequired = false)]
		public string Formatter
		{
			get
			{
				return (string)this[FormatterPropertyName];
			}
			set
			{
				this[FormatterPropertyName] = value;
			}
		}

		[ConfigurationProperty(HeaderPropertyName, DefaultValue = "----------------------------------------", IsRequired = false)]
		public string Header
		{
			get
			{
				return (string)this[HeaderPropertyName];
			}
			set
			{
				this[HeaderPropertyName] = value;
			}
		}

		[ConfigurationProperty(TimeStampPatternPropertyName, DefaultValue = "yyyy-MM-dd")]
		public string TimeStampPattern
		{
			get
			{
				return (string)this[TimeStampPatternPropertyName];
			}
			set
			{
				this[TimeStampPatternPropertyName] = value;
			}
		}

		[ConfigurationProperty(RollMessagePropertyName, IsRequired = false)]
		public string RollMessage
		{
			get
			{
				return (string)this[RollMessagePropertyName];
			}
			set
			{
				this[RollMessagePropertyName] = value;
			}
		}

		public CustomRollingFlatFileTraceListenerData()
		  : base(typeof(CustomRollingFlatFileTraceListener))
		{
			ListenerDataType = typeof(CustomRollingFlatFileTraceListenerData);
		}

		public CustomRollingFlatFileTraceListenerData(string name, string fileName, string header, string footer, string timeStampPattern, TraceOptions traceOutputOptions, string formatter)
		  : base(name, typeof(CustomRollingFlatFileTraceListener), traceOutputOptions)
		{
			FileName = fileName;
			Header = header;
			Footer = footer;
			TimeStampPattern = timeStampPattern;
			Formatter = formatter;
		}

		public CustomRollingFlatFileTraceListenerData(string name, string fileName, string header, string footer, string timeStampPattern, TraceOptions traceOutputOptions, string formatter, SourceLevels filter)
		  : base(name, typeof(CustomRollingFlatFileTraceListener), traceOutputOptions, filter)
		{
			FileName = fileName;
			Header = header;
			Footer = footer;
			TimeStampPattern = timeStampPattern;
			Formatter = formatter;
		}

		protected override TraceListener CoreBuildTraceListener(LoggingSettings settings) => new CustomRollingFlatFileTraceListener(FileName, Header, Footer, BuildFormatterSafe(settings, Formatter), TimeStampPattern, RollMessage);
	}

	[ConfigurationElementType(typeof(CustomRollingFlatFileTraceListenerData))]
	public class CustomRollingFlatFileTraceListener : FlatFileTraceListener
	{
		private const string DefaultSeparator = "----------------------------------------";
		private readonly string fileName;
		private readonly string timeStampPattern;
		private readonly string rollMessage;
		private bool disposed;

		public CustomRollingFlatFileTraceListener(string fileName, string header = DefaultSeparator, string footer = DefaultSeparator, ILogFormatter formatter = null, string timeStampPattern = "yyyy-MM-dd", string rollMessage = null)
		  : base(ComputeFileName(fileName, DateTime.Today, timeStampPattern), header, footer, formatter)
		{
			Guard.ArgumentNotNullOrEmpty(fileName, nameof(fileName));
			this.fileName = fileName;
			this.timeStampPattern = timeStampPattern;
			this.rollMessage = rollMessage;
		}

		~CustomRollingFlatFileTraceListener()
		{
			Dispose(false);
		}

		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
			RollIfNecessary();
			if (Writer == null) return;
			base.TraceData(eventCache, source, eventType, id, data);
		}

		protected override void Dispose(bool disposing)
		{
			if (this.disposed) return;

			if (disposing)
			{
				base.Dispose(disposing);
			}

			this.disposed = true;
		}

		private void RollIfNecessary()
		{
			var newFileName = CheckIsRollNecessary();
			if (string.IsNullOrEmpty(newFileName)) return;
			PerformRoll(newFileName);
			WriteRollMessage();
		}

		private string CheckIsRollNecessary()
		{
			var currentFileName = ((FileStream)((StreamWriter)Writer).BaseStream).Name;
			var newFileName = ComputeFileName(this.fileName, DateTime.Today, timeStampPattern);
			return currentFileName == newFileName ? null : newFileName;
		}

		private static string ComputeFileName(string fileName, DateTime currentDateTime, string timeStampPattern)
		{
			var directoryName = Path.GetDirectoryName(fileName);
			var withoutExtension = Path.GetFileNameWithoutExtension(fileName);
			var extension = Path.GetExtension(fileName);
			var stringBuilder = new StringBuilder(withoutExtension);
			LoggerContextFormatter.Format(stringBuilder);
			if (!string.IsNullOrEmpty(timeStampPattern))
			{
				stringBuilder.Append(currentDateTime.ToString(timeStampPattern, CultureInfo.InvariantCulture));
			}
			stringBuilder.Append(extension);
			return Path.GetFullPath(Path.Combine(directoryName, stringBuilder.ToString()));
		}

		private void PerformRoll(string newFileName)
		{
			Writer.Close();
			Writer = CreateWriter(newFileName);
		}

		private static TextWriter CreateWriter(string fileName)
		{
			TextWriter result;

			// StreamWriter by default uses UTF8Encoding which will throw on invalid encoding errors.
			// This can cause the internal StreamWriter's state to be irrecoverable. It is bad for tracing 
			// APIs to throw on encoding errors. Instead, we should provide a "?" replacement fallback  
			// encoding to substitute illegal chars. For ex, In case of high surrogate character 
			// D800-DBFF without a following low surrogate character DC00-DFFF
			// NOTE: We also need to use an encoding that does't emit BOM whic is StreamWriter's default
			var encodingWithFallback = GetEncodingWithFallback(new UTF8Encoding(false));
			var path = Path.GetFullPath(fileName);
			try
			{
				result = new StreamWriter(path, true, encodingWithFallback, 4096);
			}
			catch (Exception)
			{
				result = null;
			}

			return result;
		}

		private static Encoding GetEncodingWithFallback(Encoding encoding)
		{
			// Clone it and set the "?" replacement fallback
			var result = (Encoding)encoding.Clone();
			result.EncoderFallback = EncoderFallback.ReplacementFallback;
			result.DecoderFallback = DecoderFallback.ReplacementFallback;
			return result;
		}

		private void WriteRollMessage()
		{
			if (string.IsNullOrEmpty(this.rollMessage)) return;
			if (Formatter == null) return; // Consider calling base.TraceData instead of Write in this case.

			var message = LoggerContextFormatter.Format(this.rollMessage);
			var logEntry = new LogEntry { Message = message };
			WriteLine(Formatter.Format(logEntry));
		}
	}
}