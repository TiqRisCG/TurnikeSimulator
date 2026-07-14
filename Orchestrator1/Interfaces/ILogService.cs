public interface ILogService
{
    Task WriteLogAsync(
        string code,
        string userName,
        string result);
}