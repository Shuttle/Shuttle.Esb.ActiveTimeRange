namespace Shuttle.Esb.ActiveTimeRange
{
	public class ActiveTimeRangeOptions
	{
		public const string SectionName = "Shuttle:Modules:ActiveTimeRange";

		public string ActiveFromTime { get; set; } = "*";
		public string ActiveToTime { get; set; } = "*";
	}
}