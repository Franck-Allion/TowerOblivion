namespace TowerOblivion.Core
{
    public interface ILogger
    {
        void Info(string category, string code, string message);
        void Warn(string category, string code, string message);
        void Error(string category, string code, string message);
    }
}
