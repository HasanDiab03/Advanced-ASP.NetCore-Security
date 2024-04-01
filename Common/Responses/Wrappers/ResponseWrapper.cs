namespace Common.Responses.Wrappers
{
	public interface IResponseWrapper
	{
		public bool IsSuccessful { get; set; }
		public string Error { get; set; }
	}
	public class ResponseWrapper<T> : IResponseWrapper
	{
		public bool IsSuccessful { get; set; }
		public T ResponseData { get; set; }
		public string Error { get; set; }
		public static ResponseWrapper<T> Success(T data) => new() { ResponseData = data, IsSuccessful = true };
		public static ResponseWrapper<T> Fail(string error) => new() { Error = error, IsSuccessful = false};
	}
}
