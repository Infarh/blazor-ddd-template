using SolutionTemplate.BlazorUI.Infrastructure.Commands.Base;

namespace SolutionTemplate.BlazorUI.Infrastructure.Commands;

public class LambdaCommandAsync : CommandAsync
{
    private readonly ActionAsync<object?> _Execute;
    private readonly Func<object?, bool>? _CanExecute;

    public LambdaCommandAsync(ActionAsync Execute, Func<bool>? CanExecute = null) : this(_ => Execute(), CanExecute is null ? null : _ => CanExecute()) { }

    public LambdaCommandAsync(ActionAsync<object?> Execute, Func<object?, bool>? CanExecute = null)
    {
        _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
        _CanExecute = CanExecute;
    }

    protected override bool CanExecute(object? p) => base.CanExecute(p) && (_CanExecute?.Invoke(p) ?? true);

    protected override Task ExecuteAsync(object? parameter) => _Execute(parameter);
}
