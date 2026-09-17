namespace boostorder_ecommerce.GlobalStates
{
    public class UserState
    {
        public string Username { get; private set; } = string.Empty;
        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Username);

        public event Action? OnStateChanged;

        public void Login(string username)
        {
            Username = username;
            NotifyStateChanged();
        }

        public void Logout()
        {
            Username = string.Empty;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}
