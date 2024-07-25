namespace OnlineDataExchange.Interface
{
    public interface IFailedLoginTracker
    {
        void IncrementFailedAttempts(string username);
        int GetFailedAttempts(string username);
        void ResetFailedAttempts(string username);

        public bool IsAccountLocked(string username);
        public void LockAccount(string username);
        public void UnlockAccount(string username);

    }

}
