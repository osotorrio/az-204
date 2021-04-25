using System;

namespace EventSender
{
    public class ActivityData
    {
        public Guid Id { get; set; }
        public string CorrelationId { get; set; }
        public string OperationName { get; set; }
        public string Status { get; set; }
        public string EventCategory { get; set; }
        public string Level { get; set; }
        public DateTime Date { get; set; }

        public string Subscription { get; set; }
        public string InitiatedBy { get; set; }

        public string ResourceType { get; set; }
        public string ResourceGroup { get; set; }
        public string Resource { get; set; }

        public override string ToString()
        {
            return ($"Id : {Id}, Operation Name : {OperationName}");
        }
    }
}