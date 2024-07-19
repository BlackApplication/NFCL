namespace Services.States.Interfaces;

public interface IDownloadState {
    event Action OnChagePrecent;
    void Restart(long allBytes);
    void AddBytes(long bytes);
    int Precent();
}
