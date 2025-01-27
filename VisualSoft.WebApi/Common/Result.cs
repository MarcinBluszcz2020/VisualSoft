using System.Text.Json.Serialization;

namespace VisualSoft.WebApi.Common;


/// <summary>
/// Represents operation result.
/// </summary>
public partial class Result
{
	private IEnumerable<string>? _errors;

	/// <summary>
	/// A flag indicating that the operation was successful.
	/// </summary>
	[JsonIgnore]
	public virtual bool IsSuccess => State == ResultType.Success;

	/// <summary>
	/// A flag indicating that the operation was failed.
	/// </summary>
	[JsonIgnore]
	public bool IsFailed => State == ResultType.Failed;

	/// <summary>
	/// A flag indicating that the operation was cancelled.
	/// </summary>
	[JsonIgnore]
	public bool IsCancelled => State == ResultType.Cancelled;

	/// <summary>
	/// Operation status.
	/// </summary>
	public ResultType State { get; init; }

	/// <summary>
	/// Operation errors (if occured).
	/// </summary>
	public IEnumerable<string>? Errors
	{
		get
		{
			var exceptionMessage = Exception?.Message;

			if (!string.IsNullOrEmpty(exceptionMessage))
			{
				if (_errors != null)
				{
					return _errors.Concat(new[] { exceptionMessage });
				}
				else
				{
					return new[] { exceptionMessage };
				}
			}
			else
			{
				return _errors;
			}
		}
		init => _errors = value;
	}

	/// <summary>
	/// Operation exception (if occured) - not serialized.
	/// </summary>
	[JsonIgnore]
	public Exception? Exception { get; init; }

	public Result()
	{
	}

	/// <summary>
	/// Returns failed operation result.
	/// </summary>
	/// <param name="errors">Collection of occured errors.</param>
	/// <param name="exception">Occured exception.</param>
	/// <returns>Failed operation result.</returns>
	public static Result Failed(IEnumerable<string>? errors = default, Exception? exception = default)
	{
		return new Result
		{
			State = ResultType.Failed,
			Exception = exception,
			_errors = errors,
		};
	}

	/// <summary>
	/// Returns failed operation result.
	/// </summary>
	/// <param name="exception">Occured exception.</param>
	/// <param name="errors">Occured errors.</param>
	/// <returns>Failed operation result.</returns>
	public static Result Failed(Exception? exception, params string[] errors)
	{
		return new Result
		{
			State = ResultType.Failed,
			Exception = exception,
			_errors = errors,
		};
	}

	/// <summary>
	/// Returns failed operation result.
	/// </summary>
	/// <param name="errors">Occured errors.</param>
	/// <returns>Failed operation result.</returns>
	public static Result Failed(params string[] errors)
	{
		return new Result
		{
			State = ResultType.Failed,
			_errors = errors,
		};
	}

	/// <summary>
	/// Returns successful operation result.
	/// </summary>
	/// <returns>Successful operation result.</returns>
	public static Result Successful()
	{
		return new Result
		{
			State = ResultType.Success
		};
	}

	/// <summary>
	/// Returns cancelled operation result.
	/// </summary>
	/// <returns>Cancelled operation result.</returns>
	public static Result Cancelled()
	{
		return new Result
		{
			State = ResultType.Cancelled
		};
	}

	/// <summary>
	/// Returns failed operation result.
	/// </summary>
	/// <typeparam name="T">Type of operation result data.</typeparam>
	/// <param name="errors">Error(s) occured during operation.</param>
	/// <param name="exception">Exception occured during operation.</param>
	/// <returns>Failed operation result.</returns>
	public static Result<T> Failed<T>(IEnumerable<string>? errors = default, Exception? exception = default)
	{
		return new Result<T>
		{
			State = ResultType.Failed,
			Data = default,
			_errors = errors,
			Exception = exception,
		};
	}

	/// <summary>
	/// Returns failed operation result.
	/// </summary>
	/// <typeparam name="T">Type of operation result data.</typeparam>
	/// <param name="exception">Exception occured during operation.</param>
	/// <param name="errors">Error(s) occured during operation.</param>
	/// <returns>Failed operation result.</returns>
	public static Result<T> Failed<T>(Exception? exception, params string[] errors)
	{
		return new Result<T>
		{
			State = ResultType.Failed,
			Data = default,
			_errors = errors,
			Exception = exception,
		};
	}

	/// <summary>
	/// Returns failed operation result.
	/// </summary>
	/// <typeparam name="T">Type of operation result data.</typeparam>
	/// <param name="errors">Error(s) occured during operation.</param>
	/// <returns>Failed operation result.</returns>
	public static Result<T> Failed<T>(params string[] errors)
	{
		return new Result<T>
		{
			State = ResultType.Failed,
			Data = default,
			_errors = errors,
		};
	}

	/// <summary>
	/// Returns successful operation result.
	/// </summary>
	/// <typeparam name="T">Type of operation result data.</typeparam>
	/// <param name="data">Operation result data.</param>
	/// <returns>Successful operation result</returns>
	public static Result<T> Successful<T>(T data)
	{
		return new Result<T>
		{
			State = ResultType.Success,
			Data = data
		};
	}

	/// <summary>
	/// Returns cancelled operation result.
	/// </summary>
	/// <typeparam name="T">Type of operation result data.</typeparam>
	/// <returns>Cancelled operation result</returns>
	public static Result<T> Cancelled<T>()
	{
		return new Result<T>
		{
			State = ResultType.Cancelled,
			Data = default
		};
	}

	/// <summary>
	/// Converts result without data.
	/// </summary>
	/// <typeparam name="TConvert">Type of converted result data type</typeparam>
	/// <returns>Converted result</returns>
	public Result<TConvert> ConvertWithoutData<TConvert>()
	{
		return new Result<TConvert>
		{
			_errors = this._errors,
			Exception = this.Exception,
			State = this.State,
		};
	}

	/// <summary>
	/// Returns formatted error message.
	/// </summary>
	/// <returns>Error message</returns>
	public string GetErrorMessage()
	{
		if (Errors is null)
		{
			return string.Empty;
		}

		return string.Join(';', Errors);
	}
}