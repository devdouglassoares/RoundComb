namespace Core.Testing.Runner
{
    public enum IntegrationTestResultState
    {
        Inconclusive,
        NotRunnable,
        Skipped,
        Ignored,
        Success,
        Failure,
        Error,
        Cancelled,
    }
}