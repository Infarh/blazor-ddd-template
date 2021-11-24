namespace SolutionTemplate.BlazorUI.Infrastructure.Commands.Base;

public abstract class CommandAsync : Command
{
    private Task? _CommandTask;

    #region LastException : Exception - Исключение, возникшее в процессе выполнения команды

    /// <summary>Исключение, возникшее в процессе выполнения команды</summary>
    private Exception? _LastException;

    /// <summary>Исключение, возникшее в процессе выполнения команды</summary>
    public Exception? LastException { get => _LastException; set => Set(ref _LastException, value); }

    #endregion

    public bool InvokeAsync { get; init; }

    protected override bool CanExecute(object? parameter) => base.CanExecute(parameter) && _CommandTask is null;

    protected override async void Execute(object? parameter)
    {
        try
        {
            var task = InvokeAsync ? Task.Run(() => ExecuteAsync(parameter)) : ExecuteAsync(parameter);
            _CommandTask = task;
            await task;
        }
        catch (Exception error)
        {
            LastException = error;
        }

        _CommandTask = null;
    }

    protected abstract Task ExecuteAsync(object? parameter);
}
