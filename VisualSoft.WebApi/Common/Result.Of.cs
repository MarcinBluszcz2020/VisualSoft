namespace VisualSoft.WebApi.Common;

public partial class Result
{
	/// <summary>
	/// Executes and returns result of operation.
	/// </summary>
	/// <param name="operation">Operation to execute</param>
	/// <returns>Result of operation</returns>
	public static Result Of(Action operation)
	{
		try
		{
			operation();

			return Successful();
		}
		catch (OperationCanceledException)
		{
			return Cancelled();
		}
		catch (Exception ex)
		{
			return Failed(ex);
		}
	}

	/// <summary>
	/// Executes and returns result of operation.
	/// </summary>
	/// <param name="operation">Operation to execute</param>
	/// <returns>Result of operation</returns>
	public static Result Of(Func<Result> operation)
	{
		Result result;

		try
		{
			result = operation();
		}
		catch (OperationCanceledException)
		{
			result = Cancelled();
		}
		catch (Exception ex)
		{
			result = Failed(ex);
		}

		return result;
	}

	/// <summary>
	/// Executes and returns result of operation.
	/// </summary>
	/// <param name="operation">Operation to execute</param>
	/// <returns>Result of operation</returns>
	public static async Task<Result> Of(Func<Task> operation)
	{
		try
		{
			await operation();

			return Successful();
		}
		catch (OperationCanceledException)
		{
			return Cancelled();
		}
		catch (Exception ex)
		{
			return Failed(ex);
		}
	}

	/// <summary>
	/// Executes and returns result of operation.
	/// </summary>
	/// <param name="operation">Operation to execute</param>
	/// <returns>Result of operation</returns>
	public static async Task<Result> Of(Func<Task<Result>> operation)
	{
		Result result;

		try
		{
			result = await operation();
		}
		catch (OperationCanceledException)
		{
			result = Cancelled();
		}
		catch (Exception ex)
		{
			result = Failed(ex);
		}

		return result;
	}

	/// <summary>
	/// Executes and returns result of operation.
	/// </summary>
	/// <param name="operation">Operation to execute</param>
	/// <returns>Result of operation</returns>
	public static Result<TData> Of<TData>(Func<TData> operation)
	{
		try
		{
			var resultData = operation();

			return Successful(resultData);
		}
		catch (OperationCanceledException)
		{
			return Cancelled<TData>();
		}
		catch (Exception ex)
		{
			return Failed<TData>(ex);
		}
	}

	/// <summary>
	/// Executes and returns result of operation.
	/// </summary>
	/// <param name="operation">Operation to execute</param>
	/// <returns>Result of operation</returns>
	public static Result<TData> Of<TData>(Func<Result<TData>> operation)
	{
		Result<TData> result;

		try
		{
			result = operation();
		}
		catch (OperationCanceledException)
		{
			result = Cancelled<TData>();
		}
		catch (Exception ex)
		{
			result = Failed<TData>(ex);
		}

		return result;
	}

	/// <summary>
	/// Executes and returns result of operation.
	/// </summary>
	/// <param name="operation">Operation to execute</param>
	/// <returns>Result of operation</returns>
	public static async Task<Result<TData>> Of<TData>(Func<Task<TData>> operation)
	{
		try
		{
			var resultData = await operation();

			return Successful(resultData);
		}
		catch (OperationCanceledException)
		{
			return Cancelled<TData>();
		}
		catch (Exception ex)
		{
			return Failed<TData>(ex);
		}
	}

	/// <summary>
	/// Executes and returns result of operation.
	/// </summary>
	/// <param name="operation">Operation to execute</param>
	/// <returns>Result of operation</returns>
	public static async Task<Result<TData>> Of<TData>(Func<Task<Result<TData>>> operation)
	{
		Result<TData> result;

		try
		{
			result = await operation();
		}
		catch (OperationCanceledException)
		{
			result = Cancelled<TData>();
		}
		catch (Exception ex)
		{
			result = Failed<TData>(ex);
		}

		return result;
	}

	/// <summary>
	/// Aggregates and returns result of many operations.
	/// </summary>
	/// <returns>Result of operation</returns>
	public static async Task<Result<TData[]>> OfMany<TData>(IEnumerable<Task<Result<TData>>> resultTasks)
	{
		Result<TData[]> result;

		try
		{
			var manyResults = await Task.WhenAll(resultTasks);

			if (manyResults.All(x => x.IsSuccess))
			{
				return Successful(manyResults.Select(x => x.Data!).ToArray());
			}
			else if (manyResults.Any(x => x.IsFailed))
			{
				var errors = manyResults.Where(x => x._errors is not null).SelectMany(x => x._errors!).ToArray();
				var exceptions = manyResults.Where(x => x.Exception is not null).Select(x => x.Exception!).ToArray();

				var resultException = exceptions.Length > 0 ? new AggregateException(exceptions) : null;

				return Failed<TData[]>(errors, resultException);
			}
			else
			{
				return Cancelled<TData[]>();
			}
		}
		catch (OperationCanceledException)
		{
			result = Cancelled<TData[]>();
		}
		catch (Exception ex)
		{
			result = Failed<TData[]>(ex);
		}

		return result;
	}

	/// <summary>
	/// Aggregates and returns result of many operations.
	/// </summary>
	/// <returns>Result of operation</returns>
	public static Result<TData[]> OfMany<TData>(IEnumerable<Result<TData>> manyResults)
	{
		if (manyResults.All(x => x.IsSuccess))
		{
			return Successful(manyResults.Select(x => x.Data!).ToArray());
		}
		else if (manyResults.Any(x => x.IsFailed))
		{
			var errors = manyResults.Where(x => x._errors is not null).SelectMany(x => x._errors!).ToArray();
			var exceptions = manyResults.Where(x => x.Exception is not null).Select(x => x.Exception!).ToArray();

			var resultException = exceptions.Length > 0 ? new AggregateException(exceptions) : null;

			return Failed<TData[]>(errors, resultException);
		}
		else
		{
			return Cancelled<TData[]>();
		}
	}
}