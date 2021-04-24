using System;

namespace CustomEventTopicSender
{
    public class GridEvent
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public string EventType { get; set; }
        public string Data { get; set; }
        public DateTime EventTime { get; set; }
    }
}