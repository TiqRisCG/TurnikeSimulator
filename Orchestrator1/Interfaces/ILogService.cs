namespace Orchestrator.Interfaces
{
    //log yazabilen servisler
    public interface ILogService
    {
        void WriteLog(
            string code,
            string userName,
            string result
            );
    }
}
