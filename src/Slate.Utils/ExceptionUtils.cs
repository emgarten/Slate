using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Slate.Utils
{
    public static class ExceptionUtils
    {
        /// <summary>
        /// [Type] Exception message
        /// </summary>
        public static string GetExceptionMessage(Exception ex)
        {
            return GetExceptionMessage(ex, showType: true, message: null);
        }

        /// <summary>
        /// [Type] Exception message
        /// </summary>
        public static string GetExceptionMessage(Exception ex, bool showType)
        {
            return GetExceptionMessage(ex, showType, message: null);
        }

        /// <summary>
        /// Message
        ///   - [Type] Exception message
        ///   - [Type] Exception message
        /// </summary>
        /// <remarks>Displays exceptions top level if no message is given.</remarks>
        public static string GetExceptionMessage(Exception ex, bool showType, string message)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            var sb = new StringBuilder();
            var hasMessage = !string.IsNullOrEmpty(message);

            var exceptions = FlattenExceptions(ex).ToList();

            if (hasMessage)
            {
                sb.AppendLine(message);
            }

            foreach (var exception in exceptions)
            {
                var exMessage = showType ? FormatExceptionWithName(exception) : exception.Message;

                if (hasMessage)
                {
                    sb.AppendLine($"\t- {exMessage}");
                }
                else
                {
                    sb.AppendLine(exMessage);
                }
            }

            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// Return the root exception thrown.
        /// </summary>
        public static Exception Unwrap(Exception ex)
        {
            return FlattenExceptions(ex).FirstOrDefault();
        }

        /// <summary>
        /// Flatten AggregateExceptions
        /// </summary>
        public static IEnumerable<Exception> FlattenExceptions(Exception ex)
        {
            if (ex != null)
            {
                if (ex is AggregateException ag)
                {
                    return ag.InnerExceptions.SelectMany(e => FlattenExceptions(e));
                }
                else if (ex is TargetInvocationException te)
                {
                    return FlattenExceptions(te.InnerException);
                }
                else
                {
                    return new[] { ex };
                }
            }

            return Enumerable.Empty<Exception>();
        }

        /// <summary>
        /// [Type] Message
        /// </summary>
        private static string FormatExceptionWithName(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            return $"[{ex.GetType()}] {ex.Message}";
        }
    }
}
