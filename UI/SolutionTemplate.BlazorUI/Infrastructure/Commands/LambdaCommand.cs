using SolutionTemplate.BlazorUI.Infrastructure.Commands.Base;

namespace SolutionTemplate.BlazorUI.Infrastructure.Commands;

public class LambdaCommand : Command
{
    private readonly Action<object?> _Execute;
    private readonly Func<object?, bool>? _CanExecute;

    public LambdaCommand(Action Execute, Func<bool>? CanExecute = null) : this(_ => Execute(), CanExecute is null ? null : _ => CanExecute()) { }

    public LambdaCommand(Action<object?> Execute, Func<object?, bool>? CanExecute = null)
    {
        _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
        _CanExecute = CanExecute;
    }

    protected override bool CanExecute(object? p) => base.CanExecute(p) && (_CanExecute?.Invoke(p) ?? true);

    protected override void Execute(object? parameter) => _Execute(parameter);
}
