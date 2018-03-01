public void AllowFailure(Action action, Action<Exception> onException = null)
{
    try
    {
        action();
    }
    catch (Exception ex)
    {
        if(onException != null)
        {
            onException(ex);
        }
    }
}