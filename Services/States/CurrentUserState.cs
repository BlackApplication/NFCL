using Models.Api;
using Services.States.Interfaces;

namespace Services.States;

public class CurrentUserState : ICurrentUserState {
    private CurrentUserModel? _currentUser;
    public CurrentUserModel? CurrentUser {
        get => _currentUser;
        set => _currentUser = value;
    }
}
