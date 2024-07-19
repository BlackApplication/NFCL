using Services.States.Interfaces;

namespace Services.States;

public class DownloadState : IDownloadState {
    public event Action OnChagePrecent = null!;

    private long AllBytes { get; set; } = 0;
    private long DownloadedBytes { get; set; } = 0;

    private DateTime LastUpdate = DateTime.MinValue;

    public void Restart(long allBytes) {
        AllBytes = allBytes;
        DownloadedBytes = 0;
    }

    public void AddBytes(long bytes) {
        DownloadedBytes += bytes;

        var now = DateTime.Now;
        if ((now - LastUpdate).TotalMilliseconds > 100) {
            LastUpdate = now;
            OnChagePrecent?.Invoke();
        }
    }

    public int Precent() {
        return (int)(DownloadedBytes * 100 / AllBytes);
    }
}
