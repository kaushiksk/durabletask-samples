namespace DurableTaskSamples.Common.Logging
{
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Schema;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class DtfEventFormatter : IEventTextFormatter
    {
        private static HashSet<string> AllowedPayloads = new HashSet<string>()
        {
            "message",
            "eventType",
        };

        public void WriteEvent(EventEntry eventEntry, TextWriter writer)
        {
            writer.WriteLine(FormatPayload(eventEntry));
        }

        private static string FormatPayload(EventEntry entry)
        {
            EventSchema schema = entry.Schema;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("[{0}] ", entry.GetFormattedTimestamp("hh:mm:ss"));

            for (int i = 0; i < entry.Payload.Count; i++)
            {
                if (AllowedPayloads.Contains(schema.Payload[i]))
                {
                    string payload = entry.Payload[i].ToString();
                    if (payload.TrimStart().StartsWith("Exception"))
                    {
                        stringBuilder.Append(payload.Substring(0, 200));
                    }
                    else
                    {
                        stringBuilder.Append(payload);
                    }
                }

            }

            return stringBuilder.ToString();
        }
    }
}
