namespace Common.Responses.Wrappers
{
	public class ResponseWrapper<T>
	{
		public bool IsSuccessful { get; set; }
		public T ResponseData { get; set; }
		public string Error { get; set; }
		public static ResponseWrapper<T> Success(T data) => new() { ResponseData = data, IsSuccessful = true };
		public static ResponseWrapper<T> Fail(string error) => new() { Error = error, IsSuccessful = false};
	}
}
