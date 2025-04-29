using System;

namespace linq_slideviews;

public record VisitRecord
{
	public int UserId { get; init; }
	public int SlideId { get; init; }
	public TimeSpan Time { get; init; }
	public DateTime Date { get; init; }

	public VisitRecord(int userId, int slideId, TimeSpan time, DateTime date)
	{
		UserId = userId;
		SlideId = slideId;
		Time = time;
		Date = date;
	}

	public override string ToString() =>
		$"{nameof(UserId)}: {UserId}, {nameof(SlideId)}: {SlideId}, {nameof(Time)}: {Time}, {nameof(Date)}: {Date}";

	public override int GetHashCode()
	{
		unchecked
		{
			var hashCode = UserId;
			hashCode = (hashCode * 397) ^ SlideId;
			hashCode = (hashCode * 397) ^ Time.GetHashCode();
			hashCode = (hashCode * 397) ^ Date.GetHashCode();
			return hashCode;
		}
	}
}
