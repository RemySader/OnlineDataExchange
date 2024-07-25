using OnlineDataExchange.Interface;

namespace OnlineDataExchange.Repository
{
    public class FailedLoginTracker : IFailedLoginTracker
    {
        private Dictionary<string, int> _failedAttempts = new Dictionary<string, int>();

        private readonly Dictionary<string, (bool IsLocked, DateTime LockExpiration)> _lockedAccounts = new Dictionary<string, (bool, DateTime)>();
        private readonly TimeSpan _lockDuration = TimeSpan.FromMinutes(0.1);

        public void IncrementFailedAttempts(string username)
        {
            if (_failedAttempts.ContainsKey(username))
            {
                _failedAttempts[username]++;
            }
            else
            {
                _failedAttempts[username] = 1;
            }
        }

        public int GetFailedAttempts(string username)
        {
            return _failedAttempts.ContainsKey(username) ? _failedAttempts[username] : 0;
        }

        public void ResetFailedAttempts(string username)
        {
            if (_failedAttempts.ContainsKey(username))
            {
                _failedAttempts.Remove(username);
            }
        }


        public bool IsAccountLocked(string username)
        {
            lock (_lockedAccounts)
            {
                if (_lockedAccounts.TryGetValue(username, out var lockInfo))
                {
                    return lockInfo.IsLocked && lockInfo.LockExpiration > DateTime.UtcNow;
                }
                return false;
            }
        }

        public void LockAccount(string username)
        {
            lock (_lockedAccounts)
            {
                _lockedAccounts[username] = (true, DateTime.UtcNow.Add(_lockDuration));
            }
        }

        public void UnlockAccount(string username)
        {
            lock (_lockedAccounts)
            {
                if (_lockedAccounts.TryGetValue(username, out var lockInfo))
                {
                    if (lockInfo.LockExpiration <= DateTime.UtcNow)
                    {
                        _lockedAccounts.Remove(username);
                    }
                }
            }
        }
    }

}
