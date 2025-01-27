using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace VisualSoft.WebApi.Common;

/// <summary>
/// Represents operation result with data.
/// </summary>
/// <typeparam name="T">Result data type.</typeparam>
public class Result<T> : Result
{
	/// <summary>
	/// Operation result data.
	/// </summary>
	public T? Data { get; init; }

	/// <summary>
	/// A flag indicating that the operation was successful.
	/// </summary>
	[MemberNotNullWhen(true, nameof(Data))]
	[JsonIgnore]
	public override bool IsSuccess => base.IsSuccess;

	public Result()
	{
	}
}