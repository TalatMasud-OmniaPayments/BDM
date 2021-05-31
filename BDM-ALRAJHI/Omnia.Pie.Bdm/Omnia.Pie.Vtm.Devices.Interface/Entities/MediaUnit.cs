namespace Omnia.Pie.Vtm.Devices.Interface.Entities
{
	public class MediaUnit
	{
		public IMediaDevice MediaDevice { get; set; }

		public int Id { get; set; }
		public string Type { get; set; }
		public string Currency { get; set; }
		public int Value { get; set; }
		public string Status { get; set; }
        public string UnitStatus { get; set; }
        public string UnitId { get; set; }

        public int InitialCount { get; set; }
		public int Count { get; set; }
        public string CountTitle { get; set; }
        public string RejectTitle { get; set; }
        public string RetractTitle { get; set; }
        public string TotalTitle { get; set; }
        public int DispensedCount { get; set; }
		public int PresentedCount { get; set; }
		public int RejectedCount { get; set; }
        
        public int RetractedCount { get; set; }
        
        public int RemainingCount => Count + RejectedCount;
		public int TotalCount { get; set; }
       
        public int MaxCount { get; set; }
	}
}