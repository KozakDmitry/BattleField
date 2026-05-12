using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

public class DIWaiter
{
    public RegisterMode mode;
    public UniTaskCompletionSource awaiter;
    public CancellationTokenRegistration token;
    public DIWaiter(RegisterMode mode, UniTaskCompletionSource awaiter)
    {
        this.mode = mode;
        this.awaiter = awaiter;
    }
}