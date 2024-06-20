using Core.States.Interfaces;
using Models.Api;

namespace Services.States;

public class CurrentUserState : ICurrentUserState {
    private CurrentUserModel? _currentUser;
    public CurrentUserModel? CurrentUser {
        get => _currentUser;
        set => _currentUser = value;
    }
}
