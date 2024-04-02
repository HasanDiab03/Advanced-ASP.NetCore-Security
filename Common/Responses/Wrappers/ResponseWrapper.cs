namespace Common.Responses.Wrappers
{
	public interface IResponseWrapper
	{
		public bool IsSuccessful { get; set; }
		public List<string> Errors { get; set; }
	}
	public class ResponseWrapper<T> : IResponseWrapper
	{
		public bool IsSuccessful { get; set; }
		public T ResponseData { get; set; }
		public List<string> Errors { get; set; }
		public static ResponseWrapper<T> Success(T data) => new() { ResponseData = data, IsSuccessful = true };
		public static ResponseWrapper<T> Fail(string error) => new() { Errors = new () { error }, IsSuccessful = false};
		public static ResponseWrapper<T> Fail(List<string> errors) => new() { Errors = errors, IsSuccessful = false };
	}
}
